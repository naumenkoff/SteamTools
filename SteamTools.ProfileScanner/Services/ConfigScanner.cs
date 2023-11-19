using System.Text.RegularExpressions;
using SteamTools.Domain.Models;
using SteamTools.Domain.Providers;
using SteamTools.Infrastructure.Models;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner.Services;

public class ConfigScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly SteamClient _steamClient;

    public ConfigScanner(SteamClient steamClient, ITemplateProvider<IScanner> scannerPatternProvider)
    {
        _steamClient = steamClient;
        _pattern = scannerPatternProvider.GetTemplate(this);
    }

    public IEnumerable<LocalResult> GetProfiles()
    {
        var configFile = _steamClient.GetConfigFile();
        if (configFile is null) return Enumerable.Empty<ConfigData>();

        var file = new FileMatcher<IEnumerable<Match>>(configFile);
        return from match in file.GetMatch(_pattern) where match.Success select CreateConfigData(match);
    }

    private static ConfigData CreateConfigData(Match match)
    {
        var steamProfile = new SteamProfile(long.Parse(match.Groups[2].Value));
        return new ConfigData(steamProfile, LocalResultType.Config)
        {
            Login = match.Groups[1].Value
        };
    }
}