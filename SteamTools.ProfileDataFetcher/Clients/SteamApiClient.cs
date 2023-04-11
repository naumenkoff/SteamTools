using Newtonsoft.Json;
using SteamTools.Core.Enums;
using SteamTools.Core.Services;
using SteamTools.ProfileDataFetcher.Models;
using SteamTools.ProfileDataFetcher.Providers;
using JsonException = System.Text.Json.JsonException;

namespace SteamTools.ProfileDataFetcher.Clients;

public class SteamApiClient : ISteamApiClient
{
    private readonly ISteamApiKeyProvider _apiKey;
    private readonly HttpClient _httpClient;
    private readonly INotificationService _notificationService;

    public SteamApiClient(ISteamApiKeyProvider steamApiKey, HttpClient httpClient,
        INotificationService notificationService)
    {
        _apiKey = steamApiKey;
        _httpClient = httpClient;
        _notificationService = notificationService;
    }

    public async Task<ResolvedVanityUrl> ResolveVanityUrlAsync(string vanityUrl)
    {
        _notificationService.ShowNotification("Starting to process Vanity Url", NotificationLevel.Common);
        var uri =
            $"https://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key={_apiKey.GetSteamApiKey()}&vanityurl={vanityUrl}";
        try
        {
            using var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            _notificationService.ShowNotification("Received response from Steam API", NotificationLevel.Common);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GenericSteamResponse<ResolvedVanityUrl>>(content);
            return result.Response;
        }
        catch (HttpRequestException httpRequestException)
        {
            _notificationService.ShowNotification(
                $"An error {httpRequestException.StatusCode} occurred while processing VanityUrl",
                NotificationLevel.Warning);
            return new ResolvedVanityUrl();
        }
        catch (JsonException)
        {
            _notificationService.ShowNotification("An error occurred during the parsing of VanityUrl",
                NotificationLevel.Warning);
            return new ResolvedVanityUrl();
        }
    }

    public async Task<PlayerSummaries> GetPlayerSummariesAsync(SteamID64 steamId64)
    {
        _notificationService.ShowNotification("Starting to obtain additional information about the profile",
            NotificationLevel.Common);
        var uri =
            $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_apiKey.GetSteamApiKey()}&steamids={steamId64.ID64}";
        try
        {
            using var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            _notificationService.ShowNotification("Received response from Steam API", NotificationLevel.Common);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GenericSteamResponse<PlayerSummariesResponse>>(content);
            return result.Response?.Players.FirstOrDefault() ?? new PlayerSummaries();
        }
        catch (HttpRequestException httpRequestException)
        {
            _notificationService.ShowNotification(
                $"An error {httpRequestException.StatusCode} occurred while obtaining additional information about the profile",
                NotificationLevel.Warning);
            return new PlayerSummaries();
        }
        catch (JsonException)
        {
            _notificationService.ShowNotification(
                "An error occurred while parsing additional information about the profile", NotificationLevel.Warning);
            return new PlayerSummaries();
        }
    }
}