using System.Text.RegularExpressions;
using SteamTools.Domain.Models;
using SteamTools.Domain.Providers;
using SteamTools.Infrastructure.Models;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner.Services;

public class LoginusersScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly SteamClient _steamClient;

    public LoginusersScanner(SteamClient steamClient, ITemplateProvider<IScanner> scannerPatternProvider)
    {
        _steamClient = steamClient;
        _pattern = scannerPatternProvider.GetTemplate(this);
    }

    public IEnumerable<LocalResult> GetProfiles()
    {
        var loginusers = _steamClient.GetLoginusersFile();
        if (loginusers is null) return Enumerable.Empty<LoginusersData>();

        var file = new FileMatcher<IEnumerable<Match>>(loginusers);
        return from match in file.GetMatch(_pattern) where match.Success select CreateLoginusersData(match);
    }

    private static LoginusersData CreateLoginusersData(Match match)
    {
        var steamProfile = new SteamProfile(long.Parse(match.Groups[1].Value));
        var time = long.Parse(match.Groups[3].Captures[7].Value);
        return new LoginusersData(steamProfile, LocalResultType.Loginusers)
        {
            Login = match.Groups[3].Captures[0].Value,
            Name = match.Groups[3].Captures[1].Value,
            Timestamp = DateTimeOffset.FromUnixTimeSeconds(time).ToLocalTime()
        };
    }
}