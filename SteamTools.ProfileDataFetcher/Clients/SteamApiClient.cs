using Newtonsoft.Json;
using SteamTools.Core.Models;
using SteamTools.ProfileDataFetcher.Models;
using SteamTools.ProfileDataFetcher.Providers;
using SteamTools.ProfileDataFetcher.Services;
using JsonException = System.Text.Json.JsonException;

namespace SteamTools.ProfileDataFetcher.Clients;

public class SteamApiClient : ISteamApiClient
{
    private const string ResolveVanityUrl =
        "https://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key={0}&vanityurl={1}";

    private const string GetPlayerSummaries =
        "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}";

    private readonly ISteamApiKeyProvider _apiKey;
    private readonly ISteamApiClientCacheService _cacheService;
    private readonly HttpClient _httpClient;

    public SteamApiClient(ISteamApiKeyProvider steamApiKey, HttpClient httpClient,
        ISteamApiClientCacheService cacheService)
    {
        _apiKey = steamApiKey;
        _httpClient = httpClient;
        _cacheService = cacheService;
    }

    public async ValueTask<ResolvedVanityUrl> ResolveVanityUrlAsync(string vanityUrl)
    {
        var cachedResult = _cacheService.GetFromCache<ResolvedVanityUrl, string>(vanityUrl);
        if (cachedResult is not null) return cachedResult;

        var uri = string.Format(ResolveVanityUrl, _apiKey.GetSteamApiKey(), vanityUrl);
        var result = await GetSteamApiResponse<ResolvedVanityUrl>(uri);
        if (result is null) return new ResolvedVanityUrl();

        _cacheService.Cache(vanityUrl, result.Response);
        return result.Response;
    }

    public async ValueTask<PlayerSummaries> GetPlayerSummariesAsync(SteamID64 steamId64)
    {
        var cachedResult = _cacheService.GetFromCache<PlayerSummaries, SteamID64>(steamId64);
        if (cachedResult is not null) return cachedResult;

        var uri = string.Format(GetPlayerSummaries, _apiKey.GetSteamApiKey(), steamId64.ID64);
        var result = await GetSteamApiResponse<PlayerSummariesResponse>(uri);
        if (result is null) return new PlayerSummaries();
        var player = result.Response.Players.FirstOrDefault();
        if (player is null) return new PlayerSummaries();

        _cacheService.Cache(steamId64, player);
        return player;
    }

    private async ValueTask<GenericSteamResponse<T>> GetSteamApiResponse<T>(string uri)
    {
        try
        {
            using var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await DeserializeSteamApiResponse<T>(response);
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

    private static async ValueTask<GenericSteamResponse<T>> DeserializeSteamApiResponse<T>(
        HttpResponseMessage httpResponseMessage)
    {
        var document = await httpResponseMessage.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(document)
            ? default
            : JsonConvert.DeserializeObject<GenericSteamResponse<T>>(document);
    }
}