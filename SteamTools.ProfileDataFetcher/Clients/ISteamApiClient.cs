using SteamTools.Core.Models;
using SteamTools.ProfileDataFetcher.Models;

namespace SteamTools.ProfileDataFetcher.Clients;

public interface ISteamApiClient
{
    ValueTask<ResolvedVanityUrl> ResolveVanityUrlAsync(string vanityUrl);
    ValueTask<PlayerSummaries> GetPlayerSummariesAsync(SteamID64 steamID64);
}