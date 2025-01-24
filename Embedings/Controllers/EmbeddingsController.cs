using Embedings.Helpers;
using Embedings.Interfaces;
using Embedings.Services;
using Microsoft.AspNetCore.Mvc;

namespace Embedings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmbeddingsController : ControllerBase
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IPineconeService _pineconeService;

        public EmbeddingsController(IEmbeddingService embeddingService, IPineconeService pineconeService)
        {
            _embeddingService = embeddingService;
            _pineconeService = pineconeService;
        }

        [HttpPost("Vectorize")]
        public async Task<IActionResult> Vectorize([FromQuery] string input)
        {
            var vector = await _embeddingService.GetEmbeddingAsync(input);
            return Ok(vector);
        }

        [HttpPost("euclidean")]
        public async Task<IActionResult> Euclidean([FromQuery] string word1, string word2)
        {
            var vec1 = await _embeddingService.GetEmbeddingAsync(word1);
            var vec2 = await _embeddingService.GetEmbeddingAsync(word2);

            var distance = _embeddingService.EuclideanDistance(vec1, vec2);
            return Ok(distance);
        }

        [HttpPost("cosine")]
        public async Task<IActionResult> Cosine([FromQuery] string word1, string word2)
        {
            var vec1 = await _embeddingService.GetEmbeddingAsync(word1);
            var vec2 = await _embeddingService.GetEmbeddingAsync(word2);

            var similarity = _embeddingService.CosineSimilarity(vec1, vec2);
            return Ok(similarity);
        }

        [HttpPost("manhattan")]
        public async Task<IActionResult> Manhattan([FromQuery] string word1, string word2)
        {
            var vec1 = await _embeddingService.GetEmbeddingAsync(word1);
            var vec2 = await _embeddingService.GetEmbeddingAsync(word2);

            var distance = _embeddingService.ManhattanDistance(vec1, vec2);
            return Ok(distance);
        }

        [HttpPost("SimilarityScore")]
        public async Task<IActionResult> Compare([FromQuery] string word1, string word2, string word3)
        {
            var similarityResult = await _embeddingService.CompareWordsAsync(word1, word2, word3);

            var results = new Dictionary<string, float>
            {
                { $"{word1} - {word2}", similarityResult.Score1 },
                { $"{word1} - {word3}", similarityResult.Score2 }
            };

            return Ok(results);
        }

        [HttpGet("KNN-exact-search")]
        public async Task<IActionResult> ExactSearch([FromQuery] string input, int k)
        {
                //Generate query embedding
                var vectorizedInput = await _embeddingService.GetEmbeddingAsync(input);

                //Loading all vectors from the csv File 
                var embeddings = _pineconeService.LoadVectors();

                //Searching nearest vectors as we compare all of them
                var result = SimilaritySearchService.Search(embeddings, vectorizedInput, k);

                return Ok(result.Select(x => x.Sentence));
        }

        [HttpGet("KNN-ANN")]
        public async Task<IActionResult> IvfFlatSearch([FromQuery] string input, int k, int nprobe = 5)
        {
                //Generate query embedding
                var vectorizedInput = await _embeddingService.GetEmbeddingAsync(input);

                //Loading all vectors from the csv File 
                var embeddings = _pineconeService.LoadVectors();
                // Creating and training 
                var ivfIndex = new IndexIVFFlat(numCentroids: 20);
                ivfIndex.Train(embeddings);

                //Searching nearest vectors
                var result = ivfIndex.Search(vectorizedInput, k, nprobe);

                
                return Ok(result.Select(x => x.Sentence));
        }


    }
}
