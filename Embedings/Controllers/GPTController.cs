using Embedings.Interfaces;
using Embedings.Models;
using Microsoft.AspNetCore.Mvc;
using MSSqlServerDB;

namespace Embedings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GPTController : ControllerBase
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IPineconeService _pineconeService;
        private readonly IGPTService _gptService;
        private readonly EmbedingsDbContext _dbContext;

        public GPTController(
            IEmbeddingService embeddingService,
            IPineconeService pineconeService,
            IGPTService gptService,
            EmbedingsDbContext dbContext)
        {
            _embeddingService = embeddingService;
            _pineconeService = pineconeService;
            _gptService = gptService;
            _dbContext = dbContext;
        }

        [HttpPost("TestGPT")]
        public async Task<IActionResult> TestGPT([FromBody] GPTRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Inputs))
            {
                return BadRequest("Invalid request.");
            }

            var result = await _gptService.GetResponseAsync(request.Inputs);
            return Ok(result);
        }

        [HttpPost("GetCombinedAnswer")]
        public async Task<IActionResult> TestGPT2([FromBody] GPTRequest request)
        {
            string userQuestion = request.Inputs;

            //1.Transform question to vector.
            var questionEmbedding = await _embeddingService.GetEmbeddingAsync(userQuestion);

            //2. Searching the nearest 
            var pineconeResponse = await _pineconeService.QueryVectors(
                new QueryRequestData
                {
                    SearchInput = userQuestion,
                    TopK = 3,
                    IncludeValues = false,
                    IncludeMetadata = true
                },
                questionEmbedding
            );

            var topMatches = pineconeResponse.Matches?.ToList();
            var paragraphList = new List<string>();

            //If we store it in RDB

            //foreach (var match in topMatches)
            //{
            //    if (Guid.TryParse(match.Id, out Guid g))
            //    {
            //        var entity = await _dbContext.Embeddings.FindAsync(g);
            //        if (entity != null)
            //        {
            //            paragraphList.Add(entity.EmbeddingText);
            //        }
            //    }
            //}

            //If we store it as metadata
            foreach (var match in topMatches)
            {
                paragraphList.Add(match.Metadata.ToString());
            }

                string combinedPrompt =
                $"You are an AI assistant helping a user with information about our API. " +
                $"Below are three paragraphs retrieved from the documentation that are closely related to the user's question. " +
                $"Please rewrite the information into a single, concise, and coherent answer that directly addresses the question: {userQuestion}\n\n" +
                $"Paragraph 1: {paragraphList.ElementAtOrDefault(0)}\n" +
                $"Paragraph 2: {paragraphList.ElementAtOrDefault(1)}\n" +
                $"Paragraph 3: {paragraphList.ElementAtOrDefault(2)}";

            var combinedAnswer = await _gptService.GetResponseAsync4(combinedPrompt);

            return Ok(combinedAnswer);
        }
    }

}

