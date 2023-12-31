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

        var content = ByteVdfParser.Parse(file);
        var accountsNode = content.AllContainers.Get("Accounts");
        if (accountsNode is null) yield break;
        
        foreach (var account in accountsNode.Containers)
        {
            if (account.Objects.AsInt64("SteamID", out var id64) is not true) continue;
            var profile = new SteamProfile(id64.Value);
            yield return new ConfigData(profile, LocalResultType.Config)
            {
                Login = account.Key
            };
        }
    }
}