using SProject.VDF;
using SProject.VDF.Extensions;
using SteamTools.Common;
using SteamTools.ProfileScanner.Abstractions;
using SteamTools.ProfileScanner.Models.ScanningResults;

namespace SteamTools.ProfileScanner.Services.Scanners;

internal sealed class ConfigScanner(SteamClient steamClient) : IScanner
{
    public IEnumerable<ResultBase> EnumerateProfiles()
    {
        var file = steamClient.GetConfigFile();
        if (file is null) yield break;

        var content = ValveDataFileParser.Parse(file);
        if (!content.HasSections) yield break;

        var accountsNode = content.Sections.FirstOrDefault("Accounts");
        if (accountsNode is null) yield break;

        foreach (var account in accountsNode.Sections)
        {
            if (!account.Properties.AsInt64("SteamID", out var id64)) continue;

            var profile = new SteamProfile(id64);
            yield return new ConfigResult(profile) { Login = account.Key };
        }
    }
}