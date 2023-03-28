using Newtonsoft.Json;
using SteamTools.ProfileDataFetcher.Models;
using SteamTools.ProfileDataFetcher.Providers;
using JsonException = System.Text.Json.JsonException;

namespace SteamTools.ProfileDataFetcher.Clients;

public class SteamHttpClient : ISteamHttpClient
{
    private readonly ISteamApiKeyProvider _apiKey;
    private readonly HttpClient _httpClient;

    public SteamHttpClient(ISteamApiKeyProvider steamApiKey)
    {
        _apiKey = steamApiKey;
        _httpClient = new HttpClient();
    }

    public async Task<ResolvedVanityUrl> ResolveVanityUrlAsync(string vanityUrl)
    {
        var uri =
            $"https://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key={_apiKey.GetSteamApiKey()}&vanityurl={vanityUrl}";
        try
        {
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GenericSteamResponse<ResolvedVanityUrl>>(content);
            return result.Response;
        }
        catch (HttpRequestException)
        {
            return new ResolvedVanityUrl();
        }
        catch (JsonException)
        {
            return new ResolvedVanityUrl();
        }
    }

    public async Task<PlayerSummaries> GetPlayerSummariesAsync(long steamId64)
    {
        var uri =
            $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_apiKey.GetSteamApiKey()}&steamids={steamId64}";
        try
        {
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GenericSteamResponse<PlayerSummariesResponse>>(content);
            return result.Response?.Players.FirstOrDefault() ?? new PlayerSummaries();
        }
        catch (HttpRequestException)
        {
            return new PlayerSummaries();
        }
        catch (JsonException)
        {
            return new PlayerSummaries();
        }
    }
}