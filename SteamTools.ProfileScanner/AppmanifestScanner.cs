using SProject.VDF;
using SteamTools.Common;

namespace SteamTools.ProfileScanner;

internal class AppmanifestScanner(SteamClient steamClient) : IScanner
{
    public IEnumerable<LocalResult> GetProfiles()
    {
        if (steamClient.Steam is null) yield break;
        foreach (var file in steamClient.Steam.GetSteamLibraries().Select(x => x.GetSteamappsDirectory()).OfType<DirectoryInfo>()
                     .SelectMany(x => x.EnumerateFiles()))
        {
            var appstate = ByteVdfParser.Parse(file).Root;
            if (appstate?.Objects.AsInt64("LastOwner", out var id64) is not true) continue;

            var profile = new SteamProfile(id64);
            yield return new AppmanifestData(profile, LocalResultType.Appmanifest)
            {
                Name = appstate.Objects.Get("name")?.Value
            };
        }
    }
}