using SteamTools.ProfileDataFetcher.Models;

namespace SteamTools.ProfileDataFetcher.Clients;

public interface ISteamApiClient
{
    Task<ResolvedVanityUrl> ResolveVanityUrlAsync(string vanityUrl);
    Task<PlayerSummaries> GetPlayerSummariesAsync(SteamID64 steamID64);
}