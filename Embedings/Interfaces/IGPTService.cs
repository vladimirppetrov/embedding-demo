namespace Embedings.Interfaces;

public interface IGPTService
{
    Task<string> GetResponseAsync(string prompt);
    Task<string> GetResponseAsync2(string prompt);
    Task<string> GetResponseAsync3(string prompt);
    Task<string> GetResponseAsync4(string prompt);
}
