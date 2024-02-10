using SProject.VDF;
using SteamTools.Common;

namespace SteamTools.ProfileScanner;

internal class ConfigScanner(SteamClient steamClient) : IScanner
{
    public IEnumerable<LocalResult> GetProfiles()
    {
        var file = steamClient.GetConfigFile();
        if (file is null) yield break;

        var content = ByteVdfParser.Parse(file);
        var accountsNode = content.AllContainers.Get("Accounts");
        if (accountsNode is null) yield break;

        foreach (var account in accountsNode.Containers)
        {
            if (account.Objects.AsInt64("SteamID", out var id64) is not true) continue;
            var profile = new SteamProfile(id64);
            yield return new ConfigData(profile, LocalResultType.Config)
            {
                Login = account.Key
            };
        }
    }
}