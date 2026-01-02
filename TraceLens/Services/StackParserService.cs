using System.Text.RegularExpressions;
using TraceLens.Models;

namespace TraceLens.Services;

public class StackParserService
{
    private List<StackNode> StackNodes { get; set; }

    public StackParserService()
    {
        StackNodes = new List<StackNode>();
    }

    public ParsedStackTrace ParseStackTrace(string stackTrace)
    {
        StackNodes.Clear();
        var parsed = new ParsedStackTrace();

        parsed.MainCause = ExtractMainCause(stackTrace);
        
        var parts = stackTrace.Split(new[] { "at " }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var line = part.Trim();
            if (string.IsNullOrEmpty(line))
                continue;

            if (!line.Contains("("))
                continue;

            string methodPart, filePart = null;
            int lineNumber = 0;

            var split = line.Split(" in ");
            methodPart = split[0].Trim();

            if (split.Length > 1)
            {
                filePart = split[1].Trim();
                var lineSplit = filePart.Split(":line");
                filePart = lineSplit[0].Trim();
                if (lineSplit.Length > 1)
                    int.TryParse(lineSplit[1].Trim(), out lineNumber);
            }

            bool isUserCode = !string.IsNullOrEmpty(filePart) && !filePart.Contains("Microsoft") && lineNumber > 0;

            StackNode stackNode = new StackNode
            {
                Method = methodPart,
                File = filePart,
                Line = lineNumber,
                IsUserCode = isUserCode
            };

            StackNodes.Add(stackNode);
        }

        parsed.StackNodes = new List<StackNode>(StackNodes);

        return parsed;
    }
    
    private string ExtractMainCause(string stackTrace)
    {
        var lines = stackTrace
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            
            if (trimmed.StartsWith("at "))
                continue;
            
            if (trimmed.StartsWith("fail:") || trimmed.StartsWith("warn:") || trimmed.StartsWith("info:"))
                continue;
            
            return trimmed;
        }

        return "Unknown exception";
    }
}