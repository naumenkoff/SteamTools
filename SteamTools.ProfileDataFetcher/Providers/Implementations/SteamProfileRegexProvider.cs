using System.Text.RegularExpressions;
using SteamTools.ProfileDataFetcher.Enumerations;
using SteamTools.ProfileDataFetcher.Providers.Interfaces;

namespace SteamTools.ProfileDataFetcher.Providers.Implementations;

public partial class SteamProfileRegexProvider : ISteamProfileRegexProvider
{
    private readonly Dictionary<SteamProfileType, Regex> _regexTemplates;

    public SteamProfileRegexProvider()
    {
        _regexTemplates = GetRegexTemplates();
    }

    public Regex GetRegex(SteamProfileType steamProfileType)
    {
        return _regexTemplates[steamProfileType];
    }

    private static Dictionary<SteamProfileType, Regex> GetRegexTemplates()
    {
        return new Dictionary<SteamProfileType, Regex>
        {
            { SteamProfileType.Url, CreateSteamCustomUrlRegex() },
            { SteamProfileType.ID, CreateSteamIDRegex() },
            { SteamProfileType.ID3, CreateSteamID3Regex() },
            { SteamProfileType.ID32, CreateSteamID32Regex() },
            { SteamProfileType.ID64, CreateSteamID64Regex() },
            { SteamProfileType.Unknown, CreateUnknownRegex() }
        };
    }

    [GeneratedRegex("(76561[1-2][0-9]{11})")]
    private static partial Regex CreateSteamID64Regex();

    [GeneratedRegex("[0-9]+")]
    private static partial Regex CreateSteamID32Regex();

    [GeneratedRegex("U:[1]:([0-9]+)", RegexOptions.IgnoreCase)]
    private static partial Regex CreateSteamID3Regex();

    [GeneratedRegex("STEAM_([0-1]):([0-1]):([0-9]+)", RegexOptions.IgnoreCase)]
    private static partial Regex CreateSteamIDRegex();

    [GeneratedRegex(@"steamcommunity\.com\/(?:id|profiles)\/(\w+)", RegexOptions.IgnoreCase)]
    private static partial Regex CreateSteamCustomUrlRegex();

    [GeneratedRegex(".+")]
    private static partial Regex CreateUnknownRegex();
}