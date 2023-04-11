using System.Text.RegularExpressions;
using SteamTools.Core.Enums;
using SteamTools.Core.Services;
using SteamTools.ProfileDataFetcher.Clients;
using SteamTools.ProfileDataFetcher.Enumerations;
using SteamTools.ProfileDataFetcher.Models;
using SteamTools.ProfileDataFetcher.Utilities;

namespace SteamTools.ProfileDataFetcher.Services;

public class SteamProfileService : ISteamProfileService
{
    private readonly INotificationService _notificationService;
    private readonly ISteamProfileTypeDetector _profileTypeDetector;
    private readonly ISteamApiClient _steamApiClient;

    public SteamProfileService(ISteamProfileTypeDetector profileTypeDetector, ISteamApiClient steamApiClient,
        INotificationService notificationService)
    {
        _profileTypeDetector = profileTypeDetector;
        _steamApiClient = steamApiClient;
        _notificationService = notificationService;
    }

    public async Task<SteamProfile> GetProfileAsync(string input)
    {
        _notificationService.ShowNotification("Starting to retrieve information about the profile",
            NotificationLevel.Common);
        if (string.IsNullOrWhiteSpace(input))
        {
            _notificationService.ShowNotification("Input should not be empty!", NotificationLevel.Warning);
            return SteamProfile.Empty;
        }

        var profileType = _profileTypeDetector.DetectSteamProfileType(input);
        var id = await GetSteamID64FromProfileTypeAsync(input, profileType);

        if (id is null)
        {
            _notificationService.ShowNotification("Error occurred while retrieving profile information",
                NotificationLevel.Warning);
            return SteamProfile.Empty;
        }

        var playerSummary = await _steamApiClient.GetPlayerSummariesAsync(id);
        return new SteamProfile(id, playerSummary, input);
    }

    private async Task<SteamID64> GetSteamID64FromProfileTypeAsync(string input, SteamProfileType profileType)
    {
        var match = _profileTypeDetector.GetCachedMatchBySteamProfileType(profileType);
        return profileType switch
        {
            SteamProfileType.ID => await GetSteamID64FromSteamIDAsync(match),
            SteamProfileType.ID3 => await GetSteamID64FromSteamID3Async(match),
            SteamProfileType.ID32 => await GetSteamID64FromSteamID32Async(match),
            SteamProfileType.ID64 => await GetSteamID64FromSteamID64Async(match),
            SteamProfileType.CustomUrl => await GetSteamID64FromCustomUrlAsync(match),
            SteamProfileType.PermanentUrl => await GetSteamID64FromPermanentUrlAsync(match),
            SteamProfileType.Unknown => await GetSteamID64FromUnknownAsync(input),
            _ => null
        };
    }

    private static Task<SteamID64> GetSteamID64FromSteamIDAsync(Match match)
    {
        return Task.FromResult(new SteamID64(SteamIDConverter.ToSteamID64(match)));
    }

    private static Task<SteamID64> GetSteamID64FromSteamID3Async(Match match)
    {
        var steamId32 = new SteamID32(uint.Parse(match.Groups["SteamID32"].Value));
        return Task.FromResult(steamId32.ToSteamID64());
    }

    private static Task<SteamID64> GetSteamID64FromSteamID32Async(Capture match)
    {
        var steamId32 = new SteamID32(uint.Parse(match.Value));
        return Task.FromResult(steamId32.ToSteamID64());
    }

    private static Task<SteamID64> GetSteamID64FromSteamID64Async(Capture match)
    {
        return Task.FromResult(new SteamID64(long.Parse(match.Value)));
    }

    private async Task<SteamID64> GetSteamID64FromCustomUrlAsync(Match match)
    {
        var response = await _steamApiClient.ResolveVanityUrlAsync(match.Groups["Name"].Value);
        return long.TryParse(response.SteamID, out var id) ? new SteamID64(id) : null;
    }

    private static Task<SteamID64> GetSteamID64FromPermanentUrlAsync(Match match)
    {
        return Task.FromResult(new SteamID64(long.Parse(match.Groups["SteamID64"].Value)));
    }

    private async Task<SteamID64> GetSteamID64FromUnknownAsync(string input)
    {
        var response = await _steamApiClient.ResolveVanityUrlAsync(input);
        return long.TryParse(response.SteamID, out var id) ? new SteamID64(id) : null;
    }
}