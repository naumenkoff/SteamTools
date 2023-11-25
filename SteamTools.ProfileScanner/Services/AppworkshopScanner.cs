using SProject.VDF;
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
        if (_steamClient.Steam is null) return Enumerable.Empty<LocalResult>();

        var results = new List<LocalResult>();
        foreach (var file in _steamClient.Steam.GetAnotherInstallations().Select(x => x.GetSteamappsDirectory())
                     .Select(SteamClient.GetWorkshopDirectory).Where(x => x is not null).SelectMany(x => x!.EnumerateFiles()))
        {
            var appWorkshop = VdfSerializer.Parse(file)["AppWorkshop"];
            if (appWorkshop is null) continue;

            var workshopItemDetails = appWorkshop["WorkshopItemDetails"]?.RootObjects;
            if (workshopItemDetails is null) continue;

            var appId = appWorkshop.GetValue<int>("appid");

            foreach (var (key, rootObject) in workshopItemDetails)
            {
                var steamProfile = new SteamProfile(rootObject.GetValue<uint>("subscribedby"));
                var profile = new AppworkshopData(steamProfile, LocalResultType.Appworkshop)
                {
                    AppId = appId
                };
                results.Add(profile);
            }
        }

        return results;
    }
}