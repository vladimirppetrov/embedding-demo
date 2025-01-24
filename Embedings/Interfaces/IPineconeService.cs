using Embedings.Models;
using Pinecone;
using static Embedings.Services.PineconeService;

namespace Embedings.Interfaces;

public interface IPineconeService
{
    Task<string> UpsertVectorAsync(float[] embedding, Metadata metadata, string? vectorId = null);
    Task<Pinecone.Vector?> FetchVector(string id);
    Task<Pinecone.QueryResponse> QueryVectors(QueryRequestData data, float[] embedding);
    Task<string> UpdateVector(UpdateRequestData data);
    Task<string> DeleteVector(string id);
    Task<List<IndexData>> ListIndexes();
    Task<Pinecone.DescribeIndexStatsResponse> IndexStats(string indexName);
    List<SentenceEmbedding> LoadVectors();
}
