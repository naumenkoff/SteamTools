﻿using SteamTools.Domain.Models;
using SteamTools.Domain.Responses;
using SteamTools.ProfileFetcher.Abstractions;

namespace SteamTools.ProfileFetcher;

public class SteamApiCacheService : ICacheService
{
    private Dictionary<long, PlayerSummaries> CachedPlayerSummaries { get; } = new Dictionary<long, PlayerSummaries>();
    private Dictionary<string, ResolvedVanityUrl> CachedResolvedVanityUrls { get; } = new Dictionary<string, ResolvedVanityUrl>();

    public void Cache<T1, T2>(T1 key, T2 response)
    {
        switch (response) // skipcq: CS-R1116
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
        }
    }

    public T1? GetFromCache<T1, T2>(T2 key)
    {
        var result = key switch
        {
            SteamID64 steamId64 when CachedPlayerSummaries.TryGetValue(steamId64.AsLong, out var playerSummaries) => playerSummaries,
            string vanityUrl when CachedResolvedVanityUrls.TryGetValue(vanityUrl, out var resolvedVanityUrl) => resolvedVanityUrl,
            _ => default(object?)
        };

        return result is T1 rtn ? rtn : default;
    }
}