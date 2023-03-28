using Microsoft.Extensions.Configuration;

namespace SteamTools.ProfileDataFetcher.Providers;

public class SteamApiKeyProvider : ISteamApiKeyProvider
{
    private readonly string _steamApiKey;

    public SteamApiKeyProvider(IConfiguration configuration)
    {
        var steamApiSection = configuration.GetSection("SteamAPI");
        _steamApiKey = steamApiSection.GetSection("ApiKey").Value;
    }

    public string GetSteamApiKey()
    {
        return _steamApiKey;
    }
}