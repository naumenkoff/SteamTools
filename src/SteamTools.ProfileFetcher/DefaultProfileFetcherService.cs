using System.Text.RegularExpressions;
using SProject.Steam;
using SteamTools.Common;

namespace SteamTools.ProfileFetcher;

internal class DefaultProfileFetcherService : IProfileFetcherService
{
    private readonly IProfileTypeResolver<SteamProfileType> _profileTypeResolver;
    private readonly ISteamApiClient _steamApiClient;

    public DefaultProfileFetcherService(IProfileTypeResolver<SteamProfileType> profileTypeResolver, ISteamApiClient steamApiClient)
    {
        _profileTypeResolver = profileTypeResolver;
        _steamApiClient = steamApiClient;
    }

    public async Task<SteamProfile> GetProfileAsync(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return SteamProfile.Empty;

        var profileType = _profileTypeResolver.ResolveProfileType(input);
        var steamProfile = await GetSteamId64FromProfileTypeAsync(input, profileType).ConfigureAwait(false);
        if (steamProfile is null) return SteamProfile.Empty;

        var playerSummary = await _steamApiClient.GetPlayerSummariesAsync(steamProfile.ID64).ConfigureAwait(false);
        return new SteamProfile(steamProfile, playerSummary, input);
    }

    private async Task<SteamProfile?> GetSteamId64FromProfileTypeAsync(string input, SteamProfileType profileType)
    {
        var match = _profileTypeResolver.GetResolvedMatch(profileType);
        if (match is null) return await GetSteamId64FromUnknownAsync(input).ConfigureAwait(false);

        return profileType switch
        {
            SteamProfileType.Id => await GetSteamId64FromSteamIdAsync(match).ConfigureAwait(false),
            SteamProfileType.Id3 => await GetSteamId64FromSteamId3Async(match).ConfigureAwait(false),
            SteamProfileType.Id32 => await GetSteamId64FromSteamId32Async(match).ConfigureAwait(false),
            SteamProfileType.Id64 => await GetSteamId64FromSteamId64Async(match).ConfigureAwait(false),
            SteamProfileType.Url => await GetSteamId64FromCustomUrlAsync(match).ConfigureAwait(false),
            SteamProfileType.Unknown => await GetSteamId64FromUnknownAsync(input).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(profileType))
        };
    }

    private static ValueTask<SteamProfile> GetSteamId64FromSteamIdAsync(Match match)
    {
        var type = byte.Parse(match.Groups[2].Value);
        var accountNumber = long.Parse(match.Groups[3].Value);
        var steamProfile = new SteamProfile(SteamConverter.ToSteamID64(type, accountNumber));
        return new ValueTask<SteamProfile>(steamProfile);
    }

    private static ValueTask<SteamProfile> GetSteamId64FromSteamId3Async(Match match)
    {
        var steamProfile = new SteamProfile(uint.Parse(match.Groups[1].Value));
        return new ValueTask<SteamProfile>(steamProfile);
    }

    private static ValueTask<SteamProfile> GetSteamId64FromSteamId32Async(Capture match)
    {
        var steamProfile = new SteamProfile(uint.Parse(match.Value));
        return new ValueTask<SteamProfile>(steamProfile);
    }

    private static ValueTask<SteamProfile> GetSteamId64FromSteamId64Async(Match match)
    {
        var steamProfile = new SteamProfile(long.Parse(match.Groups[1].Value));
        return new ValueTask<SteamProfile>(steamProfile);
    }

    private async Task<SteamProfile?> GetSteamId64FromCustomUrlAsync(Match match)
    {
        var response = await _steamApiClient.ResolveVanityUrlAsync(match.Groups[1].Value).ConfigureAwait(false);
        return long.TryParse(response?.SteamID, out var id) ? new SteamProfile(id) : null;
    }

    private async Task<SteamProfile?> GetSteamId64FromUnknownAsync(string input)
    {
        var response = await _steamApiClient.ResolveVanityUrlAsync(input).ConfigureAwait(false);
        return long.TryParse(response?.SteamID, out var id) ? new SteamProfile(id) : null;
    }
}