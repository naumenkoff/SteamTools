using System.Net.Http.Json;
using System.Text.Json;
using SteamTools.Common;

namespace SteamTools.ProfileFetcher;

internal class SteamApiClient : ISteamApiClient
{
    private const string ResolveVanityUrl = "https://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key={0}&vanityurl={1}";
    private const string GetPlayerSummaries = "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}";

    private readonly ISteamApiKeyProvider _apiKey;
    private readonly ICacheService _cacheService;
    private readonly HttpClient _httpClient;

    public SteamApiClient(ISteamApiKeyProvider steamApiKey, HttpClient httpClient, ICacheService cacheService)
    {
        _apiKey = steamApiKey;
        _httpClient = httpClient;
        _cacheService = cacheService;
    }

    public async Task<ResolvedVanityUrl?> ResolveVanityUrlAsync(string vanityUrl)
    {
        if (_apiKey.SteamApiKeySetted is false) return default;

        var cachedResult = _cacheService.GetFromCache<ResolvedVanityUrl, string>(vanityUrl);
        if (cachedResult is not null) return cachedResult;

        var uri = string.Format(ResolveVanityUrl, _apiKey.GetSteamApiKey(), vanityUrl);
        var result = await GetSteamApiResponseAsync<ResolvedVanityUrl>(uri).ConfigureAwait(false);
        if (result is null) return default;

        _cacheService.Cache(vanityUrl, result.Response);
        return result.Response;
    }

    public async Task<PlayerSummaries?> GetPlayerSummariesAsync(SteamID64 steamId64)
    {
        if (_apiKey.SteamApiKeySetted is false) return default;

        var cachedResult = _cacheService.GetFromCache<PlayerSummaries, SteamID64>(steamId64);
        if (cachedResult is not null) return cachedResult;

        var uri = string.Format(GetPlayerSummaries, _apiKey.GetSteamApiKey(), steamId64.AsLong);
        var result = await GetSteamApiResponseAsync<PlayerSummariesResponse>(uri).ConfigureAwait(false);
        var player = result?.Response.Players.FirstOrDefault();
        if (player is null) return default;

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
        catch (HttpRequestException)
        {
            return default;
        }
        catch (JsonException)
        {
            return default;
        }
    }

    private static async Task<GenericSteamResponse<T>?> DeserializeSteamApiResponseAsync<T>(HttpResponseMessage httpResponseMessage)
    {
        return await httpResponseMessage.Content.ReadFromJsonAsync<GenericSteamResponse<T>>().ConfigureAwait(false);
    }
}