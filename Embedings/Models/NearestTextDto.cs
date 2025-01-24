namespace Embedings.Models;

public class NearestTextDto
{
    public string Id { get; set; }     // Pinecone ID (Guid като стринг)
    public float Score { get; set; }   // Сходство
    public string Text { get; set; }   // Реалният текст от MS SQL
}
