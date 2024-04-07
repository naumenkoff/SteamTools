using System.Text.RegularExpressions;
using SProject.Steam;
using SteamTools.Common;
using SteamTools.ProfileFetcher.Abstractions;
using SteamTools.ProfileFetcher.Enums;

namespace SteamTools.ProfileFetcher.Services;

internal sealed class ProfileTypeResolver(ITemplateProvider<SteamProfileType> templateProvider) : IProfileTypeResolver<SteamProfileType>
{
    private readonly Dictionary<SteamProfileType, Match> _matches = new();

    public Match? GetResolvedMatch(SteamProfileType steamProfileType)
    {
        return _matches.GetValueOrDefault(steamProfileType);
    }

    public SteamProfileType ResolveProfileType(string input)
    {
        return input switch
        {
            _ when IsMatch(input, SteamProfileType.Id) => SteamProfileType.Id,
            _ when IsMatch(input, SteamProfileType.Id3, match => SteamIDValidator.IsSteamID32(match.Groups[1].Value)) =>
                SteamProfileType.Id3,
            _ when IsMatch(input, SteamProfileType.Url, match => SteamIDValidator.IsSteamID64(match.Groups[1].Value),
                SteamProfileType.Id64) => SteamProfileType.Id64,
            _ when IsMatch(input, SteamProfileType.Id64, match => SteamIDValidator.IsSteamID64(match.Groups[1].Value)) =>
                SteamProfileType.Id64,
            _ when IsMatch(input, SteamProfileType.Url, match => !string.IsNullOrWhiteSpace(match.Groups[1].Value)) => SteamProfileType
                .Url,
            _ when IsMatch(input, SteamProfileType.Id32, match => SteamIDValidator.IsSteamID32(match.Value)) => SteamProfileType.Id32,
            _ => SteamProfileType.Unknown
        };
    }

    private bool IsMatch(string input, SteamProfileType steamProfileType, Predicate<Match>? predicate = default,
        SteamProfileType? targetProfileType = default)
    {
        var match = templateProvider.GetTemplate(steamProfileType).Match(input);
        return match.Success && (predicate?.Invoke(match) ?? true) && _matches.TryAdd(targetProfileType ?? steamProfileType, match);
    }
}