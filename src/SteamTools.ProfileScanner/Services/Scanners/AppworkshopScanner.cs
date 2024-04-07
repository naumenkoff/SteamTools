using SProject.VDF;
using SProject.VDF.Extensions;
using SteamTools.Common;
using SteamTools.ProfileScanner.Abstractions;
using SteamTools.ProfileScanner.Models.ScanningResults;

namespace SteamTools.ProfileScanner.Services.Scanners;

internal sealed class AppworkshopScanner(SteamClient steamClient) : IScanner
{
    public IEnumerable<ResultBase> EnumerateProfiles()
    {
        if (steamClient.Steam is null) yield break;

        foreach (var steamLibrary in steamClient.Steam.GetSteamLibraries())
        {
            var steamapps = steamLibrary.GetSteamappsDirectory();
            if (steamapps is null) continue;

            var workshop = SteamClient.GetWorkshopDirectory(steamapps);
            if (workshop is null) continue;

            foreach (var file in workshop.EnumerateFiles())
            {
                var appworkshop = ValveDataFileParser.Parse(file);
                if (!appworkshop.HasProperties) continue;

                foreach (var subscribedby in appworkshop.Properties.Enumerate("subscribedby").SelectMany(x => x.Value.Split(',')))
                {
                    var id32 = uint.Parse(subscribedby);
                    var profile = new SteamProfile(id32);
                    yield return new AppworkshopResult(profile) { AppId = appworkshop.Properties.AsInt32("appid") };
                }
            }
        }
    }
}