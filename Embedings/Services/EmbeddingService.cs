using Embedings.Interfaces;
using Embedings.Models;
using Newtonsoft.Json.Linq;
using OpenAI.Embeddings;
using System.Text;

namespace Embedings.Services;

public class EmbeddingService : IEmbeddingService
{
    private readonly string _apiKey;
    private readonly string _modelURL;
    private readonly string _openAiKey;

    public EmbeddingService(IConfiguration configuration)
    {
        _apiKey = configuration["EmbeddingService:ApiKey"];
        _modelURL = configuration["EmbeddingService:ModelURL"];
        _openAiKey = configuration["GPTService:OpenAIModelKey"];
    }

    public async Task<float[]> GetEmbeddingAsync(string input)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

        var requestData = new { inputs = input };
        var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(_modelURL, content);
        var responseData = await response.Content.ReadAsStringAsync();
        var responseJson = JArray.Parse(responseData);

        return responseJson.ToObject<float[]>();
    }

    //public async Task<float[]> GetEmbeddingAsyncOpenAi(string input)
    //{
    //    var client = CreateEmbeddingOpenAiClient();
    //    OpenAIEmbedding embedding = await client.GenerateEmbeddingAsync(input);

    //    ReadOnlyMemory<float> vector = embedding.ToFloats();

    //    return vector.ToArray();
    //}

    public float EuclideanDistance(float[] vec1, float[] vec2)
    {
        return (float)Math.Sqrt(vec1.Zip(vec2, (v1, v2) => Math.Pow(v1 - v2, 2)).Sum());
    }

    public float CosineSimilarity(float[] vec1, float[] vec2)
    {
        var dotProduct = vec1.Zip(vec2, (v1, v2) => v1 * v2).Sum();
        var magnitude1 = Math.Sqrt(vec1.Sum(v => v * v));
        var magnitude2 = Math.Sqrt(vec2.Sum(v => v * v));
        return (float)(dotProduct / (magnitude1 * magnitude2));
    }

    public float ManhattanDistance(float[] vec1, float[] vec2)
    {
        return vec1.Zip(vec2, (v1, v2) => Math.Abs(v1 - v2)).Sum();
    }

    public async Task<SimilarityResult> CompareWordsAsync(string word1, string word2, string word3)
    {
        var embedding1 = await GetEmbeddingAsync(word1);
        var embedding2 = await GetEmbeddingAsync(word2);
        var embedding3 = await GetEmbeddingAsync(word3);

        var score1 = CosineSimilarity(embedding1, embedding2);
        var score2 = CosineSimilarity(embedding1, embedding3);

        return new SimilarityResult
        {
            Score1 = score1,
            Score2 = score2
        };
       
    }


    //This is openAI embedding model

    //private EmbeddingClient CreateEmbeddingOpenAiClient()
    //{
    //    return new EmbeddingClient("text-embedding-3-small", _openAiKey);
    //}

}