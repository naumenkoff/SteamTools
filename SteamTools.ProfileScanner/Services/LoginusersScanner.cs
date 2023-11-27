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
        if (file is null) yield break;

        var content = VdfSerializer.Parse(file)["users"]?.RootObjects;
        if (content is null) yield break;
        
        foreach (var (key, users) in content)
        {
            var id64 = long.Parse(key);
            var profile = new SteamProfile(id64);

            var time = users.GetValue<long>("Timestamp");
            yield return new LoginusersData(profile, LocalResultType.Loginusers)
            {
                Login = users.GetValue<string>("PersonaName"),
                Name = users.GetValue<string>("AccountName"),
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(time).ToLocalTime()
            };
        }
    }
}