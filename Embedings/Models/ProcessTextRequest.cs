namespace Embedings.Models;

public class ProcessTextRequest
{
    public string Input { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
}
