using SteamTools.ProfileDataFetcher.Models;

namespace SteamTools.ProfileDataFetcher.Clients;

public interface ISteamHttpClient
{
    Task<ResolvedVanityUrl> ResolveVanityUrlAsync(string vanityUrl);
    Task<PlayerSummaries> GetPlayerSummariesAsync(long steamID64);
}