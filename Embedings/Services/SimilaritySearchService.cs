using static Embedings.Services.PineconeService;

namespace Embedings.Services;

public class SimilaritySearchService
{
    public static List<SentenceEmbedding> Search(List<SentenceEmbedding> embeddings, float[] query, int k)
    {
        var distances = new Dictionary<int, float>();

        for (int i = 0; i < embeddings.Count; i++)
        {
            float distance = 0;
            for (int j = 0; j < embeddings[i].EmbeddingVector.Length; j++)
            {
                distance += (embeddings[i].EmbeddingVector[j] - query[j]) * (embeddings[i].EmbeddingVector[j] - query[j]);
            }
            distances[i] = (float)Math.Sqrt(distance);
        }

        var nearestIndices = distances.OrderBy(x => x.Value).Take(k).Select(x => x.Key).ToList();

        return nearestIndices.Select(index => embeddings[index]).ToList();
    }


}
