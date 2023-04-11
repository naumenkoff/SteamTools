using System.Text.RegularExpressions;
using SteamTools.Core.Enums;
using SteamTools.Core.Services;
using SteamTools.ProfileDataFetcher.Enumerations;

namespace SteamTools.ProfileDataFetcher.Providers;

public partial class SteamProfileRegexProvider : ISteamProfileRegexProvider
{
    private readonly Dictionary<SteamProfileType, Regex> _regexTemplates;

    public SteamProfileRegexProvider(INotificationService notificationService)
    {
        _regexTemplates = new Dictionary<SteamProfileType, Regex>
        {
            { SteamProfileType.CustomUrl, CreateSteamCustomUrlRegex() },
            { SteamProfileType.ID, CreateSteamIDRegex() },
            { SteamProfileType.ID3, CreateSteamID3Regex() },
            { SteamProfileType.ID32, CreateSteamID32Regex() },
            { SteamProfileType.ID64, CreateSteamID64Regex() },
            { SteamProfileType.PermanentUrl, CreateSteamPermanentUrlRegex() },
            { SteamProfileType.Unknown, CreateUnknownRegex() }
        };

        notificationService.ShowNotification("Prepared regular expression templates", NotificationLevel.Common);
    }

    public Regex GetRegex(SteamProfileType steamProfileType)
    {
        return _regexTemplates[steamProfileType];
    }


    [GeneratedRegex("steamcommunity.com/profiles/(?<SteamID64>[0-9]+)", RegexOptions.IgnoreCase)]
    private static partial Regex CreateSteamPermanentUrlRegex();


    [GeneratedRegex("(76561[1-2][0-9]+)")]
    private static partial Regex CreateSteamID64Regex();


    [GeneratedRegex("[0-9]+")]
    private static partial Regex CreateSteamID32Regex();


    [GeneratedRegex("U:[1]:(?<SteamID32>[0-9]+)", RegexOptions.IgnoreCase)]
    private static partial Regex CreateSteamID3Regex();


    [GeneratedRegex("STEAM_([0-1]):([0-1]):([0-9]+)", RegexOptions.IgnoreCase)]
    private static partial Regex CreateSteamIDRegex();


    [GeneratedRegex("steamcommunity.com/id/(?<Name>\\w+)", RegexOptions.IgnoreCase)]
    private static partial Regex CreateSteamCustomUrlRegex();

    [GeneratedRegex(".+")]
    private static partial Regex CreateUnknownRegex();
}