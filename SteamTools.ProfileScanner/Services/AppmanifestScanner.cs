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
        if (_steamClient.Steam is null) return Enumerable.Empty<LocalResult>();

        return from appState in _steamClient.Steam.GetAnotherInstallations()
                .Select(x => x.GetSteamappsDirectory()).OfType<DirectoryInfo>()
                .SelectMany(x => x.EnumerateFiles())
                .Select(file => VdfSerializer.Parse(file)["AppState"]).OfType<IRootObject>()
            let steamProfile = new SteamProfile(appState.GetValue<long>("LastOwner"))
            select new AppmanifestData(steamProfile, LocalResultType.Appmanifest)
            {
                Name = appState.GetValue<string>("name")
            };
    }
}