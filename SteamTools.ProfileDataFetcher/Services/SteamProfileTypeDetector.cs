using System.Text.RegularExpressions;
using SteamTools.Core.Enums;
using SteamTools.Core.Services;
using SteamTools.ProfileDataFetcher.Enumerations;
using SteamTools.ProfileDataFetcher.Providers;

namespace SteamTools.ProfileDataFetcher.Services;

public class SteamProfileTypeDetector : ISteamProfileTypeDetector
{
    private readonly Dictionary<SteamProfileType, Match> _matches = new();
    private readonly INotificationService _notificationService;
    private readonly ISteamProfileRegexProvider _regexProvider;

    public SteamProfileTypeDetector(ISteamProfileRegexProvider regexProvider, INotificationService notificationService)
    {
        _regexProvider = regexProvider;
        _notificationService = notificationService;
    }

    public Match GetCachedMatchBySteamProfileType(SteamProfileType steamProfileType)
    {
        return _matches.TryGetValue(steamProfileType, out var match) ? match : null;
    }

    public SteamProfileType DetectSteamProfileType(string input)
    {
        _notificationService.ShowNotification("Determining the profile type", NotificationLevel.Common);
        return input switch
        {
            _ when IsSteamID64Format(input) => SteamProfileType.ID64,
            _ when IsSteamID3Format(input) => SteamProfileType.ID3,
            _ when IsSteamID32Format(input) => SteamProfileType.ID32,
            _ when IsSteamIDFormat(input) => SteamProfileType.ID,
            _ when IsSteamCustomUrlFormat(input) => SteamProfileType.CustomUrl,
            _ => SteamProfileType.Unknown
        };
    }

    private bool IsSteamID64Format(string input)
    {
        var match = _regexProvider.GetRegex(SteamProfileType.ID64).Match(input);
        return match.Success && input.Length == 17 && match.Value.Length == 17 &&
               AddToMatches(SteamProfileType.ID64, match);
    }

    private bool IsSteamID32Format(string input)
    {
        var match = _regexProvider.GetRegex(SteamProfileType.ID32).Match(input);
        return match.Success && input.Length <= 10 && match.Value.Length <= 10 &&
               AddToMatches(SteamProfileType.ID32, match);
    }

    private bool IsSteamID3Format(string input)
    {
        var match = _regexProvider.GetRegex(SteamProfileType.ID3).Match(input);
        return match.Success && match.Groups["SteamID32"].Length <= 10 && AddToMatches(SteamProfileType.ID3, match);
    }

    private bool IsSteamIDFormat(string input)
    {
        var match = _regexProvider.GetRegex(SteamProfileType.ID).Match(input);
        return match.Success && match.Groups["AccountNumber"].Length <= 10 && AddToMatches(SteamProfileType.ID, match);
    }

    private bool IsSteamCustomUrlFormat(string input)
    {
        var match = _regexProvider.GetRegex(SteamProfileType.CustomUrl).Match(input);
        return match.Success && AddToMatches(SteamProfileType.CustomUrl, match);
    }

    private bool AddToMatches(SteamProfileType steamProfileType, Match match)
    {
        return _matches.TryAdd(steamProfileType, match);
    }
}