using SteamTools.Core.Models;
using SteamTools.ProfileDataFetcher.Models;

namespace SteamTools.ProfileDataFetcher.Services;

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
        switch (response)
        {
            case PlayerSummaries playerSummaries when key is SteamID64 steamID64:
            {
                CachedPlayerSummaries.TryAdd(steamID64.ID64, playerSummaries);
                break;
            }
            case ResolvedVanityUrl resolvedVanityUrl when key is string vanityUrl:
            {
                CachedResolvedVanityUrls.TryAdd(vanityUrl, resolvedVanityUrl);
                break;
            }
        }
    }

    public T1 GetFromCache<T1, T2>(T2 key)
    {
        object result = key switch
        {
            SteamID64 steamID64 when CachedPlayerSummaries.TryGetValue(steamID64.ID64, out var playerSummaries) =>
                playerSummaries,
            string vanityUrl when CachedResolvedVanityUrls.TryGetValue(vanityUrl, out var resolvedVanityUrl) =>
                resolvedVanityUrl,
            _ => default
        };

        return result is T1 rtn ? rtn : default;
    }
}