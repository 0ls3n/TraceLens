namespace TraceLens.Models;

public class ParsedStackTrace
{
    public string MainCause { get; set; }
    public List<StackNode> StackNodes { get; set; } = new List<StackNode>();
}