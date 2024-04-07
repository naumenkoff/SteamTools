using SteamTools.Common;
using SteamTools.ProfileFetcher.Abstractions;
using SteamTools.ProfileFetcher.Models.Responses;

namespace SteamTools.ProfileFetcher.Services;

internal sealed class SteamApiCacheService : ICacheService
{
    private Dictionary<long, PlayerSummaries> CachedPlayerSummaries { get; } = new();
    private Dictionary<string, ResolvedVanityUrl> CachedResolvedVanityUrls { get; } = new();

    public void Cache<T1, T2>(T1 key, T2 response)
    {
        switch (response)
        {
            case PlayerSummaries playerSummaries when key is SteamID64 steamId64:
            {
                CachedPlayerSummaries.TryAdd(steamId64.AsLong, playerSummaries);
                break;
            }
            case ResolvedVanityUrl resolvedVanityUrl when key is string vanityUrl:
            {
                CachedResolvedVanityUrls.TryAdd(vanityUrl, resolvedVanityUrl);
                break;
            }
            default:
            {
                throw new InvalidOperationException();
            }
        }
    }

    public T1? GetFromCache<T1, T2>(T2 key)
    {
        return key switch
        {
            SteamID64 steamId64 when CachedPlayerSummaries.TryGetValue(steamId64.AsLong, out var playerSummaries) =>
                (T1?)(object?)playerSummaries,
            string vanityUrl when CachedResolvedVanityUrls.TryGetValue(vanityUrl, out var resolvedVanityUrl) =>
                (T1?)(object?)resolvedVanityUrl,
            _ => default
        };
    }
}