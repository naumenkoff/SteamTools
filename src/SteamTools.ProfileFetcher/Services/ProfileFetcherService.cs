using System.Text.RegularExpressions;
using SProject.Steam;
using SteamTools.Common;
using SteamTools.ProfileFetcher.Abstractions;
using SteamTools.ProfileFetcher.Enums;

namespace SteamTools.ProfileFetcher.Services;

internal sealed class ProfileFetcherService(IProfileTypeResolver<SteamProfileType> profileTypeResolver, ISteamApiClient steamApiClient)
    : IProfileFetcherService
{
    public async Task<SteamProfile> GetProfileAsync(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return SteamProfile.Empty;

        var profileType = profileTypeResolver.ResolveProfileType(input);
        var steamProfile = await GetSteamId64FromProfileTypeAsync(input, profileType).ConfigureAwait(false);
        if (steamProfile is null) return SteamProfile.Empty;

        var playerSummary = await steamApiClient.GetPlayerSummariesAsync(steamProfile.ID64).ConfigureAwait(false);
        return new SteamProfile(steamProfile, playerSummary, input);
    }

    private async Task<SteamProfile?> GetSteamId64FromProfileTypeAsync(string input, SteamProfileType profileType)
    {
        var match = profileTypeResolver.GetResolvedMatch(profileType);
        if (match is null) return await GetSteamId64FromUnknownAsync(input).ConfigureAwait(false);
        return profileType switch
        {
            SteamProfileType.Id => GetSteamId64FromSteamIdAsync(match),
            SteamProfileType.Id3 => GetSteamId64FromSteamId3Async(match),
            SteamProfileType.Id32 => GetSteamId64FromSteamId32Async(match),
            SteamProfileType.Id64 => GetSteamId64FromSteamId64Async(match),
            SteamProfileType.Url => await GetSteamId64FromCustomUrlAsync(match).ConfigureAwait(false),
            SteamProfileType.Unknown => await GetSteamId64FromUnknownAsync(input).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(profileType))
        };
    }

    private static SteamProfile GetSteamId64FromSteamIdAsync(Match match)
    {
        var type = byte.Parse(match.Groups[2].Value);
        var accountNumber = long.Parse(match.Groups[3].Value);
        return new SteamProfile(SteamConverter.ToSteamID64(type, accountNumber));
    }

    private static SteamProfile GetSteamId64FromSteamId3Async(Match match)
    {
        return new SteamProfile(uint.Parse(match.Groups[1].Value));
    }

    private static SteamProfile GetSteamId64FromSteamId32Async(Capture match)
    {
        return new SteamProfile(uint.Parse(match.Value));
    }

    private static SteamProfile GetSteamId64FromSteamId64Async(Match match)
    {
        return new SteamProfile(long.Parse(match.Groups[1].Value));
    }

    private async Task<SteamProfile?> GetSteamId64FromCustomUrlAsync(Match match)
    {
        var response = await steamApiClient.ResolveVanityUrlAsync(match.Groups[1].Value).ConfigureAwait(false);
        return long.TryParse(response?.SteamID, out var id) ? new SteamProfile(id) : default;
    }

    private async Task<SteamProfile?> GetSteamId64FromUnknownAsync(string input)
    {
        var response = await steamApiClient.ResolveVanityUrlAsync(input).ConfigureAwait(false);
        return long.TryParse(response?.SteamID, out var id) ? new SteamProfile(id) : default;
    }
}