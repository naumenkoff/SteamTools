using System.Diagnostics.CodeAnalysis;

namespace SteamTools.ProfileFetcher.Abstractions;

public interface ISteamApiKeyProvider
{
    bool SteamApiKeySetted { get; }

    [MemberNotNullWhen(true, nameof(SteamApiKeySetted))]
    string? GetSteamApiKey();
}