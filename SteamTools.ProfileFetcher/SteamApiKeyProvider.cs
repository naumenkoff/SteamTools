using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace SteamTools.ProfileFetcher;

internal class SteamApiKeyProvider : ISteamApiKeyProvider
{
    private readonly string? _steamApiKey;

    public SteamApiKeyProvider(IConfiguration configuration)
    {
        var steamApiSection = configuration.GetSection("SteamAPI");
        _steamApiKey = steamApiSection.GetSection("ApiKey").Value;
        SteamApiKeySetted = !string.IsNullOrEmpty(_steamApiKey);
    }

    public bool SteamApiKeySetted { get; }

    [MemberNotNullWhen(true, nameof(SteamApiKeySetted))]
    public string? GetSteamApiKey()
    {
        //if (_steamApiKey == null)
        //{
        //    throw new InvalidOperationException("Steam API key is not set.");
        //}

        return _steamApiKey;
    }
}