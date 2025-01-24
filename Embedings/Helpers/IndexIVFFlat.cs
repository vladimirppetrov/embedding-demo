using static Embedings.Services.PineconeService;

namespace Embedings.Helpers
{
    public class IndexIVFFlat
    {
        private readonly List<float[]> centroids = new();
        private readonly Dictionary<int, List<SentenceEmbedding>> clusters = new();
        private readonly int numCentroids;

        public IndexIVFFlat(int numCentroids)
        {
            this.numCentroids = numCentroids;
        }

        public void Train(List<SentenceEmbedding> embeddings)
        {
            for (int i = 0; i < numCentroids; i++)
            {
                centroids.Add(embeddings[i].EmbeddingVector);
                clusters[i] = new List<SentenceEmbedding>();
            }

            foreach (var embedding in embeddings)
            {
                int closestCentroid = FindClosestCentroid(embedding.EmbeddingVector);
                clusters[closestCentroid].Add(embedding);
            }
        }

        public List<SentenceEmbedding> Search(float[] query, int k, int nprobe)
        {
            var nearestCentroids = FindNearestCentroids(query, nprobe);

            var candidateVectors = new List<SentenceEmbedding>();
            foreach (var centroidIndex in nearestCentroids)
            {
                candidateVectors.AddRange(clusters[centroidIndex]);
            }

            return ExactSearch(candidateVectors, query, k);
        }

        private int FindClosestCentroid(float[] vector)
        {
            int closestIndex = -1;
            float minDistance = float.MaxValue;

            for (int i = 0; i < centroids.Count; i++)
            {
                float distance = CalculateDistance(centroids[i], vector);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        private List<int> FindNearestCentroids(float[] vector, int nprobe)
        {
            var distances = new Dictionary<int, float>();

            for (int i = 0; i < centroids.Count; i++)
            {
                float distance = CalculateDistance(centroids[i], vector);
                distances[i] = distance;
            }

            return distances.OrderBy(x => x.Value).Take(nprobe).Select(x => x.Key).ToList();
        }

        private float CalculateDistance(float[] a, float[] b)
        {
            float distance = 0;
            for (int i = 0; i < a.Length; i++)
            {
                distance += (a[i] - b[i]) * (a[i] - b[i]);
            }
            return (float)Math.Sqrt(distance);
        }

        private List<SentenceEmbedding> ExactSearch(List<SentenceEmbedding> candidates, float[] query, int k)
        {
            var distances = new Dictionary<SentenceEmbedding, float>();

            foreach (var candidate in candidates)
            {
                float distance = CalculateDistance(candidate.EmbeddingVector, query);
                distances[candidate] = distance;
            }

            return distances.OrderBy(x => x.Value).Take(k).Select(x => x.Key).ToList();
        }
    }
}
