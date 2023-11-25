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

        return from users in VdfSerializer.Parse(file)["users"]?.RootObjects
            let steamProfile = new SteamProfile(long.Parse(users.Key))
            let time = users.Value.GetValue<long>("Timestamp")
            select new LoginusersData(steamProfile, LocalResultType.Loginusers)
            {
                Login = users.Value.GetValue<string>("PersonaName"),
                Name = users.Value.GetValue<string>("AccountName"),
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(time).ToLocalTime()
            };
    }
}