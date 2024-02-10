using System.Diagnostics.CodeAnalysis;

namespace SteamTools.ProfileFetcher;

public interface ISteamApiKeyProvider
{
    bool SteamApiKeySetted { get; }

    [MemberNotNullWhen(true, nameof(SteamApiKeySetted))]
    string? GetSteamApiKey();
}