using Embedings.Models;

namespace Embedings.Interfaces;

public interface IEmbeddingService
{
    Task<float[]> GetEmbeddingAsync(string input);
    float EuclideanDistance(float[] vec1, float[] vec2);
    float CosineSimilarity(float[] vec1, float[] vec2);
    float ManhattanDistance(float[] vec1, float[] vec2);
    Task<SimilarityResult> CompareWordsAsync(string word1, string word2, string word3);
    //Task<float[]> GetEmbeddingAsyncOpenAi(string input);
}
