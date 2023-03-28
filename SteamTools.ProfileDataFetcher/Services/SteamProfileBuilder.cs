using SteamTools.ProfileDataFetcher.Clients;
using SteamTools.ProfileDataFetcher.Enumerations;
using SteamTools.ProfileDataFetcher.Models;
using SteamTools.ProfileDataFetcher.Utilities;

namespace SteamTools.ProfileDataFetcher.Services;

public class SteamProfileBuilder : ISteamProfileBuilder
{
    private readonly ISteamHttpClient _steamHttpClient;
    private readonly ISteamProfileTypeResolver _steamProfileTypeResolver;

    public SteamProfileBuilder(ISteamProfileTypeResolver steamProfileTypeResolver, ISteamHttpClient steamHttpClient)
    {
        _steamProfileTypeResolver = steamProfileTypeResolver;
        _steamHttpClient = steamHttpClient;
    }

    public async Task<SteamProfile> BuildSteamProfileAsync(string text)
    {
        var steamProfile = _steamProfileTypeResolver.ResolveSteamProfileType(text) switch
        {
            SteamProfileType.SteamID => await BuildSteamProfileFromSteamIDAsync(),
            SteamProfileType.SteamID3 => await BuildSteamProfileFromSteamID3Async(),
            SteamProfileType.SteamID32 => await BuildSteamProfileFromSteamID32Async(),
            SteamProfileType.SteamID64 => await BuildSteamProfileFromSteamID64Async(),
            SteamProfileType.SteamPermanentUrl => await BuildSteamProfileFromSteamPermanentUrlAsync(),
            SteamProfileType.SteamCustomUrl => await BuildSteamProfileFromSteamCustomUrlAsync(),
            SteamProfileType.Unknown => await BuildSteamProfileFromUnknownAsync(),
            _ => SteamProfile.Empty
        };

        return steamProfile;
    }

    private async Task<SteamProfile> BuildSteamProfileFromSteamPermanentUrlAsync()
    {
        var match = _steamProfileTypeResolver.GetCachedMatchBySteamProfileType(SteamProfileType.SteamPermanentUrl);

        var steamID64 = new SteamID64(long.Parse(match.Groups["SteamID64"].Value));
        var playerSummary = await _steamHttpClient.GetPlayerSummariesAsync(steamID64);

        return new SteamProfile(steamID64, playerSummary);
    }

    private async Task<SteamProfile> BuildSteamProfileFromSteamID64Async()
    {
        var match = _steamProfileTypeResolver.GetCachedMatchBySteamProfileType(SteamProfileType.SteamID64);

        var steamID64 = new SteamID64(long.Parse(match.Value));
        var playerSummary = await _steamHttpClient.GetPlayerSummariesAsync(steamID64);

        return new SteamProfile(steamID64, playerSummary);
    }

    private async Task<SteamProfile> BuildSteamProfileFromSteamID32Async()
    {
        var match = _steamProfileTypeResolver.GetCachedMatchBySteamProfileType(SteamProfileType.SteamID32);

        var steamID32 = new SteamID32(uint.Parse(match.Value));
        var steamID64 = steamID32.ToSteamID64();
        var playerSummary = await _steamHttpClient.GetPlayerSummariesAsync(steamID64);

        return new SteamProfile(steamID64, steamID32, playerSummary);
    }

    private async Task<SteamProfile> BuildSteamProfileFromSteamID3Async()
    {
        var match = _steamProfileTypeResolver.GetCachedMatchBySteamProfileType(SteamProfileType.SteamID3);

        var steamID32 = new SteamID32(uint.Parse(match.Groups["SteamID32"].Value));
        var steamID64 = steamID32.ToSteamID64();
        var playerSummary = await _steamHttpClient.GetPlayerSummariesAsync(steamID64);

        return new SteamProfile(steamID64, steamID32, playerSummary);
    }

    private async Task<SteamProfile> BuildSteamProfileFromSteamIDAsync()
    {
        var match = _steamProfileTypeResolver.GetCachedMatchBySteamProfileType(SteamProfileType.SteamID);

        var steamID64 = new SteamID64(SteamIDConverter.ToSteamID64(match));
        var playerSummary = await _steamHttpClient.GetPlayerSummariesAsync(steamID64);

        return new SteamProfile(steamID64, playerSummary);
    }

    private async Task<SteamProfile> BuildSteamProfileFromSteamCustomUrlAsync()
    {
        var match = _steamProfileTypeResolver.GetCachedMatchBySteamProfileType(SteamProfileType.SteamCustomUrl);

        var response = await _steamHttpClient.ResolveVanityUrlAsync(match.Groups["Name"].Value);
        if (long.TryParse(response.SteamID, out var id) is false) return SteamProfile.Empty;

        var steamID64 = new SteamID64(id);
        var playerSummary = await _steamHttpClient.GetPlayerSummariesAsync(steamID64);

        return new SteamProfile(steamID64, playerSummary);
    }

    private async Task<SteamProfile> BuildSteamProfileFromUnknownAsync()
    {
        var match = _steamProfileTypeResolver.GetCachedMatchBySteamProfileType(SteamProfileType.Unknown);

        var response = await _steamHttpClient.ResolveVanityUrlAsync(match.Value);
        if (long.TryParse(response.SteamID, out var id) is false) return SteamProfile.Empty;

        var steamID64 = new SteamID64(id);
        var playerSummary = await _steamHttpClient.GetPlayerSummariesAsync(steamID64);

        return new SteamProfile(steamID64, playerSummary);
    }
}