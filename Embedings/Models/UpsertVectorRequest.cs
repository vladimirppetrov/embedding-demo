namespace Embedings.Models;

public class UpsertVectorRequest
{
    public string Input { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
}
