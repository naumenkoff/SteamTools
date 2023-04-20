using System.Text.RegularExpressions;
using SteamTools.Core.Models;
using SteamTools.Core.Utilities;
using SteamTools.ProfileDataFetcher.Clients;
using SteamTools.ProfileDataFetcher.Enumerations;
using SteamTools.ProfileDataFetcher.Models;

namespace SteamTools.ProfileDataFetcher.Services;

public class SteamProfileService : ISteamProfileService
{
    private readonly ISteamProfileTypeDetector _profileTypeDetector;
    private readonly ISteamApiClient _steamApiClient;

    public SteamProfileService(ISteamProfileTypeDetector profileTypeDetector, ISteamApiClient steamApiClient)
    {
        _profileTypeDetector = profileTypeDetector;
        _steamApiClient = steamApiClient;
    }

    public async Task<SteamProfile> GetProfileAsync(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return SteamProfile.Empty;

        var profileType = _profileTypeDetector.DetectSteamProfileType(input);
        var id = await GetSteamID64FromProfileTypeAsync(input, profileType);

        if (id is null) return SteamProfile.Empty;

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
            SteamProfileType.Url => await GetSteamID64FromCustomUrlAsync(match),
            SteamProfileType.Unknown => await GetSteamID64FromUnknownAsync(input),
            _ => null
        };
    }

    private static Task<SteamID64> GetSteamID64FromSteamIDAsync(Match match)
    {
        var type = byte.Parse(match.Groups[2].Value);
        var accountNumber = long.Parse(match.Groups[3].Value);
        return Task.FromResult(new SteamID64(SteamIDConverter.ToSteamID64(type, accountNumber)));
    }

    private static Task<SteamID64> GetSteamID64FromSteamID3Async(Match match)
    {
        var steamId32 = new SteamID32(uint.Parse(match.Groups[1].Value));
        return Task.FromResult(steamId32.ToSteamID64());
    }

    private static Task<SteamID64> GetSteamID64FromSteamID32Async(Capture match)
    {
        var steamId32 = new SteamID32(uint.Parse(match.Value));
        return Task.FromResult(steamId32.ToSteamID64());
    }

    private static Task<SteamID64> GetSteamID64FromSteamID64Async(Match match)
    {
        return Task.FromResult(new SteamID64(long.Parse(match.Groups[1].Value)));
    }

    private async Task<SteamID64> GetSteamID64FromCustomUrlAsync(Match match)
    {
        var response = await _steamApiClient.ResolveVanityUrlAsync(match.Groups[1].Value);
        return long.TryParse(response?.SteamID, out var id) ? new SteamID64(id) : null;
    }

    private async Task<SteamID64> GetSteamID64FromUnknownAsync(string input)
    {
        var response = await _steamApiClient.ResolveVanityUrlAsync(input);
        return long.TryParse(response?.SteamID, out var id) ? new SteamID64(id) : null;
    }
}