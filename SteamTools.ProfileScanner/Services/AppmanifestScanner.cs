using SProject.VDF;
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

        var results = new List<LocalResult>();
        foreach (var file in _steamClient.Steam.GetAnotherInstallations().Select(x => x.GetSteamappsDirectory()).Where(x => x is not null)
                     .SelectMany(x => x!.EnumerateFiles()))
        {
            var appState = VdfSerializer.Parse(file)["AppState"];
            if (appState is null) continue;

            var steamProfile = new SteamProfile(appState.GetValue<long>("LastOwner"));
            var profile = new AppmanifestData(steamProfile, LocalResultType.Appmanifest)
            {
                Name = appState.GetValue<string>("name")
            };

            results.Add(profile);
        }

        return results;
    }
}