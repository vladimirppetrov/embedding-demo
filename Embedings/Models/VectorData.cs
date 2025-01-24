namespace Embedings.Models;

public class VectorData
{
    public int Id { get; set; }
    public string Sentence { get; set; }
    public double[] Embedding { get; set; }
}
