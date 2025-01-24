using Embedings.Interfaces;
using Embedings.Models;
using Microsoft.EntityFrameworkCore;
using MSSqlServerDB;
using Pinecone;

namespace Embedings.Services
{
    public class ChatBotService : IChatBotService
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IPineconeService _pineconeService;
        private readonly EmbedingsDbContext _dbContext;

        public ChatBotService(IEmbeddingService embeddingService, IPineconeService pineconeService, EmbedingsDbContext dbContext)
        {
            _embeddingService = embeddingService;
            _pineconeService = pineconeService;
            _dbContext = dbContext;
        }

        public async Task<string> ProcessTextAsync(string input, Dictionary<string, string>? metadataDict = null)
        {
            // 1. Generate embedding for the input text
            var embedding = await _embeddingService.GetEmbeddingAsync(input);

            // 2. Prepare metadata
            var metadata = new Metadata();
            //metadata["text"] = input; 
            if (metadataDict != null)
            {
                foreach (var kvp in metadataDict)
                {
                    metadata[kvp.Key] = kvp.Value;
                }
            }

            // 3. Generate GUID for both Pinecone and MS SQL
            var guid = Guid.NewGuid();

            // 4. Save the numeric expression of the input in Pinecone as vector
            var pineconeResult = await _pineconeService.UpsertVectorAsync(embedding, metadata, guid.ToString());

            // 5. Save the text expression of the input in MS SQL 
            var embeddingEntity = new MSSqlServerDB.Models.Embedding
            {
                Id = guid, 
                EmbeddingText = input
            };

            _dbContext.Embeddings.Add(embeddingEntity);
            await _dbContext.SaveChangesAsync();

            return $"Text processed and saved successfully. Pinecone ID: {guid}";
        }

        public async Task<List<NearestTextDto>> GetNearestTextsAsync(string input, uint topK = 3)
        {
            //1. Get embedding of the searched vector
            var embedding = await _embeddingService.GetEmbeddingAsync(input);

            var queryRequestData = new QueryRequestData
            {
                SearchInput = input,
                TopK = topK,
                IncludeValues = false,
                IncludeMetadata = false
            };

            //2. Query VDB for similar vectors
            var pineconeResponse = await _pineconeService.QueryVectors(queryRequestData, embedding);

            //3. Extract their guids
            var guids = pineconeResponse.Matches
                .Select(m => m.Id)
                .Select(strId => Guid.Parse(strId))
                .ToList();

            //4. Get the text representations of the similar vectors
            var matchedEmbeddings = await _dbContext.Embeddings
                .Where(e => guids.Contains(e.Id))
                .ToListAsync();

            //5. Return result
            var resultList = new List<NearestTextDto>();
            foreach (var match in pineconeResponse.Matches)
            {
                var textEntity = matchedEmbeddings.FirstOrDefault(e => e.Id == Guid.Parse(match.Id));

                var scoreValue = match.Score ?? 0.0f;

                resultList.Add(new NearestTextDto
                {
                    Id = match.Id,
                    Score = scoreValue,
                    Text = textEntity.EmbeddingText
                });
            }

            return resultList;
        }

    }
}
