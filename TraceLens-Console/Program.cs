using TraceLens.Models;
using TraceLens.Services;

namespace TraceLens_Console;

class Program
{
    static void Main(string[] args)
    {
        StackParserService service = new StackParserService();

        Console.Write("Paste stack trace: ");
        
        string trace = Console.ReadLine();        
        ParsedStackTrace parsedStackTrace = service.ParseStackTrace(trace);

        Console.WriteLine();
        
        Console.WriteLine($"Cause of problem: {parsedStackTrace.MainCause}");

        int traceNumber = 1;
        
        foreach (StackNode serviceStackNode in parsedStackTrace.StackNodes)
        {
            Console.WriteLine($"Trace no.{traceNumber}:");
            Console.WriteLine($"File: {serviceStackNode.File}");
            Console.WriteLine($"Method: {serviceStackNode.Method}");
            Console.WriteLine($"Line: {serviceStackNode.Line}");
            Console.WriteLine($"Is user code: {serviceStackNode.IsUserCode}");
            Console.WriteLine();
            traceNumber++;
        }

        Console.ReadKey();
    }
}