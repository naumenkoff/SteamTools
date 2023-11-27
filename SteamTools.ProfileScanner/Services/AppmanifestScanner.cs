using SProject.VDF;
using SProject.Vdf.Abstractions;
using SteamTools.Domain.Models;
using SteamTools.Domain.Services;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner.Services;

public class AppmanifestScanner : IScanner
{
    private readonly SteamClient _steamClient;

    public AppmanifestScanner(SteamClient steamClient)
    {
        _steamClient = steamClient;
    }

    public IEnumerable<LocalResult> GetProfiles()
    {
        if (_steamClient.Steam is null) yield break;

        foreach (var appstate in _steamClient.Steam.GetAnotherInstallations().Select(x => x.GetSteamappsDirectory()).OfType<DirectoryInfo>()
                     .SelectMany(x => x.EnumerateFiles()).Select(x => VdfSerializer.Parse(x)["AppState"]).OfType<IRootObject>())
        {
            var id64 = appstate.GetValue<long>("LastOwner");
            var profile = new SteamProfile(id64);
            yield return new AppmanifestData(profile, LocalResultType.Appmanifest)
            {
                Name = appstate.GetValue<string>("name")
            };
        }
    }
}