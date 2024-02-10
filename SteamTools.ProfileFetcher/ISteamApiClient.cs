using SteamTools.Common;

namespace SteamTools.ProfileFetcher;

public interface ISteamApiClient
{
    Task<ResolvedVanityUrl?> ResolveVanityUrlAsync(string vanityUrl);
    Task<PlayerSummaries?> GetPlayerSummariesAsync(SteamID64 steamId64);
}