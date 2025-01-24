using Embedings.Models;

namespace Embedings.Interfaces;

public interface IChatBotService
{
    Task<string> ProcessTextAsync(string input, Dictionary<string, string>? metadataDict = null);
    Task<List<NearestTextDto>> GetNearestTextsAsync(string input, uint topK = 3);
}
