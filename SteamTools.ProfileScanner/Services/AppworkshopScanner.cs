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

        foreach (var appworkshop in _steamClient.Steam.GetAnotherInstallations().Select(x => x.GetSteamappsDirectory())
                     .Select(SteamClient.GetWorkshopDirectory).OfType<DirectoryInfo>().SelectMany(x => x.EnumerateFiles()).Select(VdfSerializer.Parse)
                     .Select(x => x["AppWorkshop"]).OfType<IRootObject>())
        {
            var appId = appworkshop.GetValue<int>("appid");
            foreach (var subscriber in appworkshop.GetValueObjects("subscribedby").SelectMany(x => x.Value.Split(',')))
            {
                var id32 = uint.Parse(subscriber);
                var profile = new SteamProfile(id32);
                yield return new AppworkshopData(profile, LocalResultType.Appworkshop)
                {
                    AppId = appId
                };
            }
        }
    }
}