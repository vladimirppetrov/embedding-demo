namespace Embedings.Models;

public class GPTRequest
{
    public string Inputs { get; set; } // Текстовият prompt
    //public int MaxTokens { get; set; } = 100; // Максимален брой токени за генериране
    //public double Temperature { get; set; } = 0.7; // Креативност на модела
    //public int TopK { get; set; } = 50; // Колко думи да обмисли моделът за следваща дума
}

