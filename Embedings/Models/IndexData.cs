using OneOf;
using Pinecone;
using System.Text.Json.Serialization;

namespace Embedings.Models;

public class IndexData
{
    public required string Name { get; set; }

    public required int Dimension { get; set; }
}