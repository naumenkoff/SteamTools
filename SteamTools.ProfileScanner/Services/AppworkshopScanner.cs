using SProject.VDF;
using SProject.Vdf.Abstractions;
using SteamTools.Domain.Models;
using SteamTools.Domain.Services;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner.Services;

public class AppworkshopScanner : IScanner
{
    private readonly SteamClient _steamClient;

    public AppworkshopScanner(SteamClient steamClient)
    {
        _steamClient = steamClient;
    }

    public IEnumerable<LocalResult> GetProfiles()
    {
        if (_steamClient.Steam is null) yield break;

        foreach (var file in _steamClient.Steam.GetAnotherInstallations().Select(x => x.GetSteamappsDirectory())
                     .Select(SteamClient.GetWorkshopDirectory).OfType<DirectoryInfo>().SelectMany(x => x.EnumerateFiles()))
        {
            var appworkshop = ByteVdfParser.Parse(file);
            if (appworkshop.Empty) continue;
            
            foreach (var subscribedby in appworkshop.AllObjects.Enumerate("subscribedby").SelectMany(x => x.Value.Split(',')))
            {
                var id32 = uint.Parse(subscribedby);
                var profile = new SteamProfile(id32);
                yield return new AppworkshopData(profile, LocalResultType.Appworkshop)
                {
                    AppId = appworkshop.AllObjects.AsInt32("appid")
                };
            }
        }
    }
}