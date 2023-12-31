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
        foreach (var file in _steamClient.Steam.GetAnotherInstallations().Select(x => x.GetSteamappsDirectory()).OfType<DirectoryInfo>().SelectMany(x => x.EnumerateFiles()))
        {
            var appstate = ByteVdfParser.Parse(file).Root;
            if (appstate?.Objects.AsInt64("LastOwner", out var id64) is not true) continue;
            
            var profile = new SteamProfile(id64.Value);
            yield return new AppmanifestData(profile, LocalResultType.Appmanifest)
            {
                Name = appstate.Objects.Get("name")?.Value
            };
        }
    }
}