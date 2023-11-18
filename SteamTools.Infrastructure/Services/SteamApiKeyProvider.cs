using Microsoft.Extensions.Configuration;
using SteamTools.Domain.Providers;

namespace SteamTools.Infrastructure.Services;

public class SteamApiKeyProvider : ISteamApiKeyProvider
{
    private readonly string _steamApiKey;

    public SteamApiKeyProvider(IConfiguration configuration)
    {
        var steamApiSection = configuration.GetSection("SteamAPI");
        _steamApiKey = steamApiSection.GetSection("ApiKey").Value;
        SteamApiKeySetted = string.IsNullOrEmpty(_steamApiKey) is false;
    }

    public bool SteamApiKeySetted { get; }

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