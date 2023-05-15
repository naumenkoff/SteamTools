using Microsoft.Extensions.Configuration;
using SteamTools.ProfileDataFetcher.Providers.Interfaces;

namespace SteamTools.ProfileDataFetcher.Providers.Implementations;

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
        /*
        if (_steamApiKey == null)
        {
            throw new InvalidOperationException("Steam API key is not set.");
        }
        */

        return _steamApiKey;
    }
}