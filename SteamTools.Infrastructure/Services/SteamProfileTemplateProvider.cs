using System.Text.RegularExpressions;
using SteamTools.Domain.Enumerations;
using SteamTools.Domain.Providers;

namespace SteamTools.Infrastructure.Services;

public partial class SteamProfileTemplateProvider : ITemplateProvider<SteamProfileType>
{
    private readonly Dictionary<SteamProfileType, Regex> _regexTemplates = new Dictionary<SteamProfileType, Regex>
    {
        { SteamProfileType.Url, CreateSteamCustomUrlRegex() },
        { SteamProfileType.ID, CreateSteamIDRegex() },
        { SteamProfileType.ID3, CreateSteamID3Regex() },
        { SteamProfileType.ID32, CreateSteamID32Regex() },
        { SteamProfileType.ID64, CreateSteamID64Regex() },
        { SteamProfileType.Unknown, CreateUnknownRegex() }
    };

    public Regex GetTemplate(SteamProfileType steamProfileType)
    {
        return _regexTemplates[steamProfileType];
    }

    [GeneratedRegex("(76561[1-2][0-9]{11})")]
    private static partial Regex CreateSteamID64Regex();

    [GeneratedRegex("[0-9]+")] private static partial Regex CreateSteamID32Regex();

    [GeneratedRegex("U:[1]:([0-9]+)", RegexOptions.IgnoreCase)]
    private static partial Regex CreateSteamID3Regex();

    [GeneratedRegex("STEAM_([0-1]):([0-1]):([0-9]+)", RegexOptions.IgnoreCase)]
    private static partial Regex CreateSteamIDRegex();

    [GeneratedRegex(@"steamcommunity\.com\/(?:id|profiles)\/(\w+)", RegexOptions.IgnoreCase)]
    private static partial Regex CreateSteamCustomUrlRegex();

    [GeneratedRegex(".+")] private static partial Regex CreateUnknownRegex();
}