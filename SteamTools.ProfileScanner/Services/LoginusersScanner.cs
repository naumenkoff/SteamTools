using SProject.VDF;
using SteamTools.Domain.Models;
using SteamTools.Domain.Services;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner.Services;

public class LoginusersScanner : IScanner
{
    private readonly SteamClient _steamClient;

    public LoginusersScanner(SteamClient steamClient)
    {
        _steamClient = steamClient;
    }

    public IEnumerable<LocalResult> GetProfiles()
    {
        var file = _steamClient.GetLoginusersFile();
        if (file is null) return Enumerable.Empty<LoginusersData>();

        var users = VdfSerializer.Parse(file)["users"];
        if (users is null) return Enumerable.Empty<LocalResult>();

        var results = new List<LocalResult>();
        foreach (var (key, rootObject) in users.RootObjects)
        {
            var steamProfile = new SteamProfile(long.Parse(key));
            var time = rootObject.GetValue<long>("Timestamp");
            var profile = new LoginusersData(steamProfile, LocalResultType.Loginusers)
            {
                Login = rootObject.GetValue<string>("PersonaName"),
                Name = rootObject.GetValue<string>("AccountName"),
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(time).ToLocalTime()
            };
            results.Add(profile);
        }

        return results;
    }
}