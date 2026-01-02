using System.Text.RegularExpressions;
using TraceLens.Models;

namespace TraceLens.Services;

public class StackParserService
{
    public List<StackNode> StackNodes { get; set; }

    public StackParserService()
    {
        StackNodes = new List<StackNode>();
    }

    public ParsedStackTrace ParseStackTrace(string stackTrace)
    {
        StackNodes.Clear();
        var parsed = new ParsedStackTrace();
        
        string mainCause = null;
        
        var match = Regex.Match(stackTrace, @"(?<exception>[\w\.]+Exception: .*?)( at |$)");
        if (match.Success)
        {
            mainCause = match.Groups["exception"].Value.Trim();
        }

        if (mainCause == null)
        {
            mainCause = "Unknown exception";
        }

        parsed.MainCause = mainCause;
        
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
}