using System.Text.RegularExpressions;
using SteamTools.Core.Models;
using SteamTools.LocalProfileScanner.Models.ProfileData;

namespace SteamTools.LocalProfileScanner.Models.DataScanner;

public class LoginusersScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly ISteamClient _steamClient;

    public LoginusersScanner(ISteamClient steamClient, Regex pattern)
    {
        _steamClient = steamClient;
        _pattern = pattern;
    }

    public IEnumerable<ISteamID> GetProfiles()
    {
        if (_steamClient.LoginusersFile is null) return Enumerable.Empty<LoginusersData>();
        var file = new ManyMatchesFile(_steamClient.LoginusersFile);
        var matches = file.GetMatches(_pattern);
        return (from match in matches where match.Success select CreateLoginusersData(match)).ToList();
    }

    private static LoginusersData CreateLoginusersData(Match match)
    {
        var steamID64 = new SteamID64(long.Parse(match.Groups[1].Value));
        var login = match.Groups[3].Captures[0].Value;
        var name = match.Groups[3].Captures[1].Value;
        var time = long.Parse(match.Groups[3].Captures[7].Value);
        return new LoginusersData(steamID64, login, name, time);
    }
}