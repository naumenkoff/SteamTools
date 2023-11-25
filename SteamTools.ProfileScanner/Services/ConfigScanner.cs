using SProject.VDF;
using SteamTools.Domain.Models;
using SteamTools.Domain.Services;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner.Services;

public class ConfigScanner : IScanner
{
    private readonly SteamClient _steamClient;

    public ConfigScanner(SteamClient steamClient)
    {
        _steamClient = steamClient;
    }

    public IEnumerable<LocalResult> GetProfiles()
    {
        var file = _steamClient.GetConfigFile();
        if (file is null) return Enumerable.Empty<ConfigData>();


        return from accounts in VdfSerializer.Parse(file)
                .GetSection("Accounts")
                .SelectMany(x => x.RootObjects)
            let steamProfile = new SteamProfile(long.Parse(accounts.Value.GetValue<string>("SteamID")))
            select new ConfigData(steamProfile, LocalResultType.Config)
            {
                Login = accounts.Key
            };
    }
}