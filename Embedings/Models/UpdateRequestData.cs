namespace Embedings.Models;

public class UpdateRequestData
{
    public string Id { get; set; } 
    public float[]? Values { get; set; } 
    public Dictionary<string, string>? Metadata { get; set; } 
    public string? Namespace { get; set; } 
}
