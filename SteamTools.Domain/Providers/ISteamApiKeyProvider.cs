namespace SteamTools.Domain.Providers;

public interface ISteamApiKeyProvider
{
    bool SteamApiKeySetted { get; }
    string GetSteamApiKey();
}