using System.Text.RegularExpressions;
using SProject.Steam;
using SteamTools.Domain.Enumerations;
using SteamTools.Domain.Providers;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services;

public class SteamProfileTypeDetector : ISteamProfileTypeDetector
{
    private readonly Dictionary<SteamProfileType, Match> _matches;
    private readonly ITemplateProvider<SteamProfileType> _templateProvider;

    public SteamProfileTypeDetector(ITemplateProvider<SteamProfileType> templateProvider)
    {
        _templateProvider = templateProvider;
        _matches = new Dictionary<SteamProfileType, Match>();
    }

    public Match? GetCachedMatchBySteamProfileType(SteamProfileType steamProfileType)
    {
        return _matches.GetValueOrDefault(steamProfileType);
    }

    public SteamProfileType ResolveSteamProfileType(string input)
    {
        return input switch
        {
            _ when IsMatch(input, SteamProfileType.ID) => SteamProfileType.ID,
            _ when IsMatch(input, SteamProfileType.ID3, match => SteamIDValidator.IsSteamID32(match.Groups[1].Value)) => SteamProfileType.ID3,
            _ when IsMatch(input, SteamProfileType.Url, match => SteamIDValidator.IsSteamID64(match.Groups[1].Value), SteamProfileType.ID64) =>
                SteamProfileType.ID64,
            _ when IsMatch(input, SteamProfileType.Url, match => !string.IsNullOrWhiteSpace(match.Groups[1].Value)) => SteamProfileType.Url,
            _ when IsMatch(input, SteamProfileType.ID64, match => SteamIDValidator.IsSteamID64(match.Groups[1].Value)) => SteamProfileType.ID64,
            _ when IsSteamID32Format(input) is false => SteamProfileType.Unknown,
            _ when IsSteamID32Format(input) => SteamProfileType.ID32,
            _ => SteamProfileType.Unknown
        };
    }

    private bool IsSteamID32Format(string input)
    {
        return IsMatch(input, SteamProfileType.ID32, match => SteamIDValidator.IsSteamID32(match.Value));
    }

    private bool IsMatch(string input, SteamProfileType steamProfileType, Predicate<Match>? predicate = default,
        SteamProfileType? targetProfileType = default)
    {
        var match = _templateProvider.GetTemplate(steamProfileType).Match(input);
        return match.Success && (predicate?.Invoke(match) ?? true) && _matches.TryAdd(targetProfileType ?? steamProfileType, match);
    }
}