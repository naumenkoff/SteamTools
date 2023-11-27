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
        if (file is null) yield break;

        var content = VdfSerializer.Parse(file);
        foreach (var (key, account) in content.GetRootObjects("Accounts").SelectMany(x => x.RootObjects))
        {
            var id64 = account.GetValue<long>("SteamID");
            var profile = new SteamProfile(id64);
            yield return new ConfigData(profile, LocalResultType.Config)
            {
                Login = key
            };
        }
    }
}