namespace Embedings.Models;

public class QueryRequestData
{
    public string SearchInput { get; set; }
    public uint TopK { get; set; }
    public bool IncludeValues { get; set; } = false;
    public bool IncludeMetadata { get; set; } = true;
}