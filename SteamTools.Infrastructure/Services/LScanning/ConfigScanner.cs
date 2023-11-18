using System.Text.RegularExpressions;
using SteamTools.Domain.Models;
using SteamTools.Domain.Models.LScanning;
using SteamTools.Domain.Providers;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services.LScanning;

public class ConfigScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly ISteamClient _steamClient;

    public ConfigScanner(ISteamClient steamClient, ITemplateProvider<IScanner> scannerPatternProvider)
    {
        _steamClient = steamClient;
        _pattern = scannerPatternProvider.GetTemplate(this);
    }

    public IEnumerable<ISteamIDPair> GetProfiles()
    {
        if (_steamClient.ConfigFile is null) return Enumerable.Empty<ConfigData>();

        var file = new FileMatcher<IEnumerable<Match>>(_steamClient.ConfigFile);
        return from match in file.GetMatch(_pattern) where match.Success select CreateConfigData(match);
    }

    private static ConfigData CreateConfigData(Match match)
    {
        var steamProfile = new SteamProfile(long.Parse(match.Groups[2].Value));
        return new ConfigData(match.Groups[1].Value, steamProfile);
    }
}