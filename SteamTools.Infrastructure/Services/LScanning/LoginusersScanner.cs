using System.Text.RegularExpressions;
using SteamTools.Domain.Models;
using SteamTools.Domain.Models.LScanning;
using SteamTools.Domain.Providers;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services.LScanning;

public class LoginusersScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly ISteamClient _steamClient;

    public LoginusersScanner(ISteamClient steamClient, ITemplateProvider<IScanner> scannerPatternProvider)
    {
        _steamClient = steamClient;
        _pattern = scannerPatternProvider.GetTemplate(this);
    }

    public IEnumerable<ISteamIDPair> GetProfiles()
    {
        if (_steamClient.LoginusersFile is null) return Enumerable.Empty<LoginusersData>();

        var file = new FileMatcher<IEnumerable<Match>>(_steamClient.LoginusersFile);
        return from match in file.GetMatch(_pattern) where match.Success select CreateLoginusersData(match);
    }

    private static LoginusersData CreateLoginusersData(Match match)
    {
        var steamProfile = new SteamProfile(long.Parse(match.Groups[1].Value));
        var login = match.Groups[3].Captures[0].Value;
        var name = match.Groups[3].Captures[1].Value;
        var time = long.Parse(match.Groups[3].Captures[7].Value);
        return new LoginusersData(login, name, DateTimeOffset.FromUnixTimeSeconds(time).ToLocalTime(), steamProfile);
    }
}