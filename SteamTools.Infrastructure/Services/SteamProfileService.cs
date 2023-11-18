using System.Text.RegularExpressions;
using SProject.Steam;
using SteamTools.Domain.Enumerations;
using SteamTools.Domain.Models;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services;

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

        var profileType = _profileTypeDetector.ResolveSteamProfileType(input);
        var steamProfile = await GetSteamID64FromProfileTypeAsync(input, profileType);
        if (steamProfile is null) return SteamProfile.Empty;

        var playerSummary = await _steamApiClient.GetPlayerSummariesAsync(steamProfile.ID64);
        return new SteamProfile(steamProfile, playerSummary, input);
    }

    private async Task<SteamProfile?> GetSteamID64FromProfileTypeAsync(string input, SteamProfileType profileType)
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

    private static ValueTask<SteamProfile> GetSteamID64FromSteamIDAsync(Match match)
    {
        var type = byte.Parse(match.Groups[2].Value);
        var accountNumber = long.Parse(match.Groups[3].Value);
        var steamProfile = new SteamProfile(SteamConverter.ToSteamID64(type, accountNumber));
        return new ValueTask<SteamProfile>(steamProfile);
    }

    private static ValueTask<SteamProfile> GetSteamID64FromSteamID3Async(Match match)
    {
        var steamProfile = new SteamProfile(uint.Parse(match.Groups[1].Value));
        return new ValueTask<SteamProfile>(steamProfile);
    }

    private static ValueTask<SteamProfile> GetSteamID64FromSteamID32Async(Capture match)
    {
        var steamProfile = new SteamProfile(uint.Parse(match.Value));
        return new ValueTask<SteamProfile>(steamProfile);
    }

    private static ValueTask<SteamProfile> GetSteamID64FromSteamID64Async(Match match)
    {
        var steamProfile = new SteamProfile(long.Parse(match.Groups[1].Value));
        return new ValueTask<SteamProfile>(steamProfile);
    }

    private async Task<SteamProfile?> GetSteamID64FromCustomUrlAsync(Match match)
    {
        var response = await _steamApiClient.ResolveVanityUrlAsync(match.Groups[1].Value);
        return long.TryParse(response?.SteamID, out var id) ? new SteamProfile(id) : null;
    }

    private async Task<SteamProfile?> GetSteamID64FromUnknownAsync(string input)
    {
        var response = await _steamApiClient.ResolveVanityUrlAsync(input);
        return long.TryParse(response?.SteamID, out var id) ? new SteamProfile(id) : null;
    }
}