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

        var results = new List<LocalResult>();
        foreach (var (key, rootObject) in VdfSerializer.Parse(file).GetSection("Accounts").SelectMany(x => x.RootObjects))
        {
            var steamProfile = new SteamProfile(long.Parse(rootObject.GetValue<string>("SteamID")));
            var profile = new ConfigData(steamProfile, LocalResultType.Config)
            {
                Login = key
            };
            results.Add(profile);
            Console.WriteLine(profile.Login);
        }

        return results;
    }
}