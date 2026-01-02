namespace TraceLens.Models;

public class StackNode
{
    public string Method { get; set; }
    public string File { get; set; }
    public int Line  { get; set; }
    public bool IsUserCode { get; set; }
}