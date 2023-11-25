using System.Text.RegularExpressions;
using SteamTools.Domain.Providers;
using SteamTools.ProfileFetcher.Abstractions;

namespace SteamTools.ProfileFetcher;

public partial class ProfileTemplateProvider : ITemplateProvider<SteamProfileType>
{
    private readonly Dictionary<SteamProfileType, Regex> _regexTemplates = new Dictionary<SteamProfileType, Regex>
    {
        { SteamProfileType.Url, CreateSteamCustomUrlRegex() },
        { SteamProfileType.Id, CreateSteamIdRegex() },
        { SteamProfileType.Id3, CreateSteamId3Regex() },
        { SteamProfileType.Id32, CreateSteamId32Regex() },
        { SteamProfileType.Id64, CreateSteamId64Regex() },
        { SteamProfileType.Unknown, CreateUnknownRegex() }
    };

    public Regex GetTemplate(SteamProfileType steamProfileType)
    {
        return _regexTemplates[steamProfileType];
    }

    [GeneratedRegex("(76561[1-2][0-9]{11})", RegexOptions.Compiled)]
    private static partial Regex CreateSteamId64Regex();

    [GeneratedRegex("[0-9]+", RegexOptions.Compiled)]
    private static partial Regex CreateSteamId32Regex();

    [GeneratedRegex("U:[1]:([0-9]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex CreateSteamId3Regex();

    [GeneratedRegex("STEAM_([0-1]):([0-1]):([0-9]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex CreateSteamIdRegex();

    [GeneratedRegex(@"steamcommunity\.com\/(?:id|profiles)\/(\w+)", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex CreateSteamCustomUrlRegex();

    [GeneratedRegex(".+", RegexOptions.Compiled)]
    private static partial Regex CreateUnknownRegex();
}