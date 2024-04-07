using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SteamTools.Common;
using SteamTools.ProfileFetcher.Abstractions;
using SteamTools.ProfileFetcher.Models;
using SteamTools.ProfileFetcher.Models.Responses;

namespace SteamTools.ProfileFetcher.Services;

internal sealed class SteamApiClient : ISteamApiClient
{
    private const string ResolveVanityUrl = "https://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key={0}&vanityurl={1}";
    private const string GetPlayerSummaries = "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}";
    private readonly ICacheService _cacheService;
    private readonly HttpClient _httpClient;
    private readonly ILogger<SteamApiClient> _logger;
    private readonly string? _steamKey;
    private readonly bool _steamKeyMissing;

    public SteamApiClient(ILogger<SteamApiClient> logger, IOptions<SteamKeyOptions> steamKeyOptions, HttpClient httpClient,
        ICacheService cacheService)
    {
        _steamKey = steamKeyOptions.Value.Key;
        _steamKeyMissing = string.IsNullOrEmpty(_steamKey);
        _logger = logger;
        _httpClient = httpClient;
        _cacheService = cacheService;
    }

    public async Task<ResolvedVanityUrl?> ResolveVanityUrlAsync(string vanityUrl)
    {
        if (_steamKeyMissing)
        {
            _logger.LogDebug("The Steam API key is not specified, it's not possible to get profile information");
            return default;
        }

        var cachedResult = _cacheService.GetFromCache<ResolvedVanityUrl, string>(vanityUrl);
        if (cachedResult is not null) return cachedResult;

        var uri = string.Format(ResolveVanityUrl, _steamKey, vanityUrl);
        var result = await GetSteamApiResponseAsync<ResolvedVanityUrl>(uri).ConfigureAwait(false);
        if (result is null)
        {
            _logger.LogDebug("There's no information about profile in response from Steam API");
            return default;
        }

        _cacheService.Cache(vanityUrl, result.Response);
        return result.Response;
    }

    public async Task<PlayerSummaries?> GetPlayerSummariesAsync(SteamID64 steamId64)
    {
        if (_steamKeyMissing)
        {
            _logger.LogDebug("The Steam API key is not specified, it's not possible to get profile information");
            return default;
        }

        var cachedResult = _cacheService.GetFromCache<PlayerSummaries, SteamID64>(steamId64);
        if (cachedResult is not null) return cachedResult;

        var uri = string.Format(GetPlayerSummaries, _steamKey, steamId64.AsLong);
        var result = await GetSteamApiResponseAsync<PlayerSummariesResponse>(uri).ConfigureAwait(false);
        var player = result?.Response?.Players.FirstOrDefault();
        if (player is null)
        {
            _logger.LogDebug("There's no information about profiles in response from Steam API");
            return default;
        }

        _cacheService.Cache(steamId64, player);
        return player;
    }

    private async Task<GenericSteamResponse<T>?> GetSteamApiResponseAsync<T>(string uri)
    {
        try
        {
            using var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await DeserializeSteamApiResponseAsync<T>(response).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Couldn't get response from Steam API");
            return default;
        }
    }

    private static async Task<GenericSteamResponse<T>?> DeserializeSteamApiResponseAsync<T>(HttpResponseMessage httpResponseMessage)
    {
        using var content = httpResponseMessage.Content;
        return await content.ReadFromJsonAsync<GenericSteamResponse<T>>().ConfigureAwait(false);
    }
}