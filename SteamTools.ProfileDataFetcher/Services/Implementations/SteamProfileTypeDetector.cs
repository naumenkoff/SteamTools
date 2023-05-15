using System.Text.RegularExpressions;
using SteamTools.Core.Utilities;
using SteamTools.ProfileDataFetcher.Enumerations;
using SteamTools.ProfileDataFetcher.Providers.Interfaces;
using SteamTools.ProfileDataFetcher.Services.Interfaces;

namespace SteamTools.ProfileDataFetcher.Services.Implementations;

public class SteamProfileTypeDetector : ISteamProfileTypeDetector
{
    private readonly Dictionary<SteamProfileType, Match> _matches = new();
    private readonly ISteamProfileRegexProvider _regexProvider;

    public SteamProfileTypeDetector(ISteamProfileRegexProvider regexProvider)
    {
        _regexProvider = regexProvider;
    }

    public Match GetCachedMatchBySteamProfileType(SteamProfileType steamProfileType)
    {
        return _matches.TryGetValue(steamProfileType, out var match) ? match : default;
    }

    public SteamProfileType DetectSteamProfileType(string input)
    {
        return input switch
        {
            _ when IsSteamIDFormat(input) => SteamProfileType.ID,
            _ when IsSteamID3Format(input) => SteamProfileType.ID3,
            _ when IsSteamPermanentUrlFormat(input) => SteamProfileType.ID64,
            _ when IsSteamCustomUrlFormat(input) => SteamProfileType.Url,
            _ when IsSteamID64Format(input) => SteamProfileType.ID64,
            _ when IsSteamID32Format(input) => SteamProfileType.ID32,
            _ => SteamProfileType.Unknown
        };
    }

    private bool IsSteamID64Format(string input)
    {
        var match = _regexProvider.GetRegex(SteamProfileType.ID64).Match(input);
        return match.Success
               && SteamIDValidator.IsSteamID64(match.Groups[1].Value)
               && AddToMatches(SteamProfileType.ID64, match);
    }

    private bool IsSteamID32Format(string input)
    {
        var match = _regexProvider.GetRegex(SteamProfileType.ID32).Match(input);
        return match.Success
               && SteamIDValidator.IsSteamID32(match.Value)
               && AddToMatches(SteamProfileType.ID32, match);
    }

    private bool IsSteamID3Format(string input)
    {
        var match = _regexProvider.GetRegex(SteamProfileType.ID3).Match(input);
        return match.Success
               && SteamIDValidator.IsSteamID32(match.Groups[1].Value)
               && AddToMatches(SteamProfileType.ID3, match);
    }

    private bool IsSteamIDFormat(string input)
    {
        var match = _regexProvider.GetRegex(SteamProfileType.ID).Match(input);
        return match.Success
               && AddToMatches(SteamProfileType.ID, match);
    }

    private bool IsSteamCustomUrlFormat(string input)
    {
        var match = _regexProvider.GetRegex(SteamProfileType.Url).Match(input);
        return match.Success
               && string.IsNullOrWhiteSpace(match.Groups[1].Value) is false
               && AddToMatches(SteamProfileType.Url, match);
    }

    private bool IsSteamPermanentUrlFormat(string input)
    {
        var match = _regexProvider.GetRegex(SteamProfileType.Url).Match(input);
        return match.Success
               && SteamIDValidator.IsSteamID64(match.Groups[1].Value)
               && AddToMatches(SteamProfileType.ID64, match);
    }

    private bool AddToMatches(SteamProfileType steamProfileType, Match match)
    {
        return _matches.TryAdd(steamProfileType, match);
    }
}