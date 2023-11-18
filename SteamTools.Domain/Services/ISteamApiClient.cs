using SteamTools.Domain.Models;
using SteamTools.Domain.Responses;

namespace SteamTools.Domain.Services;

public interface ISteamApiClient
{
    ValueTask<ResolvedVanityUrl?> ResolveVanityUrlAsync(string vanityUrl);
    ValueTask<PlayerSummaries?> GetPlayerSummariesAsync(SteamID64 steamID64);
}