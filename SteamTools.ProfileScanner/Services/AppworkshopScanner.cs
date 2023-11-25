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
        if (_steamClient.Steam is null) return Enumerable.Empty<LocalResult>();

        return from appworkshop in _steamClient.Steam.GetAnotherInstallations()
                .Select(x => x.GetSteamappsDirectory())
                .Select(SteamClient.GetWorkshopDirectory).OfType<DirectoryInfo>()
                .SelectMany(x => x.EnumerateFiles())
                .Select(file => VdfSerializer.Parse(file)["AppWorkshop"]).OfType<IRootObject>()
            let appId = appworkshop.GetValue<int>("appid")
            from details in appworkshop["WorkshopItemDetails"]?.RootObjects
            let steamProfile = new SteamProfile(details.Value.GetValue<uint>("subscribedby"))
            select new AppworkshopData(steamProfile, LocalResultType.Appworkshop)
            {
                AppId = appId
            };
    }
}