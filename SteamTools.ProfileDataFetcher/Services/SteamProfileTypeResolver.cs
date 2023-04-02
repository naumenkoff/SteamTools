using System.Text.RegularExpressions;
using SteamTools.ProfileDataFetcher.Enumerations;
using SteamTools.ProfileDataFetcher.Providers;

namespace SteamTools.ProfileDataFetcher.Services;

public class SteamProfileTypeResolver : ISteamProfileTypeResolver
{
    private readonly ISteamProfileRegexProvider _steamProfileRegexProvider;
    private readonly Dictionary<SteamProfileType, Match> _templateMatches;

    public SteamProfileTypeResolver(ISteamProfileRegexProvider steamProfileRegexProvider)
    {
        _steamProfileRegexProvider = steamProfileRegexProvider;
        _templateMatches = new Dictionary<SteamProfileType, Match>();
    }

    public Match GetCachedMatchBySteamProfileType(SteamProfileType steamProfileType)
    {
        return _templateMatches[steamProfileType];
    }

    public SteamProfileType ResolveSteamProfileType(string input)
    {
        if (InSteamIDFormat(input)) return SteamProfileType.SteamID;
        if (InSteamID3Format(input)) return SteamProfileType.SteamID3;
        if (InSteamID32Format(input)) return SteamProfileType.SteamID32;
        if (InSteamID64Format(input)) return SteamProfileType.SteamID64;
        if (InSteamCustomUrlFormat(input)) return SteamProfileType.SteamCustomUrl;
        return InSteamPermanentUrlFormat(input) ? SteamProfileType.SteamPermanentUrl : InUnknownFormat(input);
    }

    private bool InSteamID64Format(string trigger)
    {
        var match = _steamProfileRegexProvider.GetRegex(SteamProfileType.SteamID64).Match(trigger);
        if (match.Success is false || trigger.Length != 17 || match.Value.Length != 17) return false;
        return _templateMatches.TryAdd(SteamProfileType.SteamID64, match);
    }

    private bool InSteamID32Format(string trigger)
    {
        var match = _steamProfileRegexProvider.GetRegex(SteamProfileType.SteamID32).Match(trigger);
        if (match.Success is false || trigger.Length > 10 || match.Value.Length > 10) return false;
        return _templateMatches.TryAdd(SteamProfileType.SteamID32, match);
    }

    private bool InSteamID3Format(string trigger)
    {
        var match = _steamProfileRegexProvider.GetRegex(SteamProfileType.SteamID3).Match(trigger);
        if (match.Success is false || match.Groups["SteamID32"].Length > 10) return false;
        return _templateMatches.TryAdd(SteamProfileType.SteamID3, match);
    }

    private bool InSteamIDFormat(string trigger)
    {
        var match = _steamProfileRegexProvider.GetRegex(SteamProfileType.SteamID).Match(trigger);
        if (match.Success is false || match.Groups["AccountNumber"].Length > 10) return false;
        return _templateMatches.TryAdd(SteamProfileType.SteamID, match);
    }

    private bool InSteamPermanentUrlFormat(string trigger)
    {
        var match = _steamProfileRegexProvider.GetRegex(SteamProfileType.SteamPermanentUrl).Match(trigger);
        if (match.Success is false || match.Groups["SteamID64"].Length != 17) return false;
        return _templateMatches.TryAdd(SteamProfileType.SteamPermanentUrl, match);
    }

    private bool InSteamCustomUrlFormat(string trigger)
    {
        var match = _steamProfileRegexProvider.GetRegex(SteamProfileType.SteamCustomUrl).Match(trigger);
        return match.Success && _templateMatches.TryAdd(SteamProfileType.SteamCustomUrl, match);
    }

    private SteamProfileType InUnknownFormat(string trigger)
    {
        var match = _steamProfileRegexProvider.GetRegex(SteamProfileType.Unknown).Match(trigger);
        _templateMatches.Add(SteamProfileType.Unknown, match);
        return SteamProfileType.Unknown;
    }
}