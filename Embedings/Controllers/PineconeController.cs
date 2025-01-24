using Embedings.Interfaces;
using Embedings.Models;
using Embedings.Services;
using Microsoft.AspNetCore.Mvc;
using MSSqlServerDB;

namespace Embedings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PineconeController : ControllerBase
    {
        private readonly EmbedingsDbContext _context;
        private readonly IPineconeService _pineconeService;
        private readonly IEmbeddingService _embeddingService;

        public PineconeController(EmbedingsDbContext context, IPineconeService pineconeService, IEmbeddingService embeddingService)
        {
            _context = context;
            _pineconeService = pineconeService;
            _embeddingService = embeddingService;
        }

        [HttpPost("UpsertVector")]
        public async Task<IActionResult> UpsertVector([FromBody] UpsertVectorRequest request)
        {
            var embedding = await _embeddingService.GetEmbeddingAsync(request.Input);
            var metadata = new Pinecone.Metadata();
            if (request.Metadata != null)
            {
                foreach (var kvp in request.Metadata)
                {
                    metadata[kvp.Key] = kvp.Value;
                }
            }
            metadata["originalText"] = request.Input;

            var result = await _pineconeService.UpsertVectorAsync(embedding, metadata);
            return Ok(result);
        }

        [HttpGet("fetch/{id}")]
        public async Task<IActionResult> FetchVector(string id)
        {
            var fetchedVector = await _pineconeService.FetchVector(id);
            if (fetchedVector == null)
            {
                return NotFound($"Vector with ID {id} not found.");
            }

            if (fetchedVector.Metadata == null)
            {
                return NotFound($"Vector with ID {id} doesn't have any metadata.");
            }

            var metadata = fetchedVector.Metadata.FirstOrDefault();
            return Ok(metadata.ToString());
        }

        [HttpPost("query")]
        public async Task<IActionResult> QueryVectors([FromBody] QueryRequestData data)
        {
            var embedding = await _embeddingService.GetEmbeddingAsync(data.SearchInput);
            var response = await _pineconeService.QueryVectors(data, embedding);

            var matches = response.Matches.FirstOrDefault();
            if (matches == null)
            {
                return NotFound("No matching vectors found.");
            }

            var result = new
            {
                Id = matches.Id,
                Score = matches.Score,
                MetaDataInfo = matches.Metadata.FirstOrDefault()
            };

            return Ok($"{result.Id} Score: {result.Score}");
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateVector([FromBody] UpdateRequestData data)
        {
            await _pineconeService.UpdateVector(data);
            return Ok("Vector updated successfully.");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteVector(string id, [FromQuery] string? namespaceName = null)
        {
            await _pineconeService.DeleteVector(id);
            return Ok("Vector deleted successfully.");
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListIndexes()
        {
            var indexes = await _pineconeService.ListIndexes();
            return Ok(indexes);
        }

        [HttpGet("index-stats/{indexName}")]
        public async Task<IActionResult> GetIndexStats(string indexName)
        {
            var stats = await _pineconeService.IndexStats(indexName);
            var info = $"Index: {indexName} = Total Vector Count: {stats.TotalVectorCount}, Dimensions: {stats.Dimension}, Fullness: {stats.IndexFullness}";
            return Ok(info);
        }

        //[HttpGet("Upsert-Documentation")]
        //public async Task<IActionResult> UpsertDocumentation(string indexName)
        //{
        //    return Ok("Documentation endpoint is under construction.");
        //}

        [HttpGet("Upsert-Csv")]
        public async Task<IActionResult> UpsertCsv(string indexName)
        {
            var vectors = _pineconeService.LoadVectors();
            return Ok("Documentation endpoint is under construction.");
        }
    }
}
