using System.Text.RegularExpressions;
using SteamTools.Core.Models;
using SteamTools.Core.Models.Steam;
using SteamTools.LocalProfileScanner.Models.ProfileData;

namespace SteamTools.LocalProfileScanner.Models.DataScanner;

public class ConfigScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly ISteamClient _steamClient;

    public ConfigScanner(ISteamClient steamClient, Regex pattern)
    {
        _steamClient = steamClient;
        _pattern = pattern;
    }

    public IEnumerable<ISteamID> GetProfiles()
    {
        if (_steamClient.ConfigFile is null) return Enumerable.Empty<ConfigData>();
        var file = new ManyMatchesFile(_steamClient.ConfigFile);
        var matches = file.GetMatches(_pattern);
        return (from match in matches where match.Success select CreateConfigData(match)).ToList();
    }

    private static ConfigData CreateConfigData(Match match)
    {
        var steam64 = new SteamID64(long.Parse(match.Groups[2].Value));
        return new ConfigData(match.Groups[1].Value, steam64, steam64.ToSteamID32());
    }
}