using SteamTools.Domain.Models;
using SteamTools.Domain.Responses;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services;

public class SteamApiClientCacheService : ISteamApiClientCacheService
{
    public SteamApiClientCacheService()
    {
        CachedPlayerSummaries = new Dictionary<long, PlayerSummaries>();
        CachedResolvedVanityUrls = new Dictionary<string, ResolvedVanityUrl>();
    }

    private Dictionary<long, PlayerSummaries> CachedPlayerSummaries { get; }
    private Dictionary<string, ResolvedVanityUrl> CachedResolvedVanityUrls { get; }

    public void Cache<T1, T2>(T1 key, T2 response)
    {
        switch (response) // skipcq: CS-R1116
        {
            case PlayerSummaries playerSummaries when key is SteamID64 steamID64:
            {
                CachedPlayerSummaries.TryAdd(steamID64.AsLong, playerSummaries);
                break;
            }
            case ResolvedVanityUrl resolvedVanityUrl when key is string vanityUrl:
            {
                CachedResolvedVanityUrls.TryAdd(vanityUrl, resolvedVanityUrl);
                break;
            }
        }
    }

    public T1? GetFromCache<T1, T2>(T2 key)
    {
        var result = key switch
        {
            SteamID64 steamID64 when CachedPlayerSummaries.TryGetValue(steamID64.AsLong, out var playerSummaries) => playerSummaries,
            string vanityUrl when CachedResolvedVanityUrls.TryGetValue(vanityUrl, out var resolvedVanityUrl) => resolvedVanityUrl,
            _ => default(object?)
        };

        return result is T1 rtn ? rtn : default;
    }
}