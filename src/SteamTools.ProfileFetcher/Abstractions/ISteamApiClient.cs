using SteamTools.Common;
using SteamTools.ProfileFetcher.Models.Responses;

namespace SteamTools.ProfileFetcher.Abstractions;

public interface ISteamApiClient
{
    Task<ResolvedVanityUrl?> ResolveVanityUrlAsync(string vanityUrl);
    Task<PlayerSummaries?> GetPlayerSummariesAsync(SteamID64 steamId64);
}