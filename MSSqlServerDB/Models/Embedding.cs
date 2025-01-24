using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSSqlServerDB.Models;

[Table("embeddings")]
public class Embedding
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } 

    [Column("embedding_text")]
    public string EmbeddingText { get; set; } 
}
