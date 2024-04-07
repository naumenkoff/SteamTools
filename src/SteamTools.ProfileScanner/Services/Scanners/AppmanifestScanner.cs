using SProject.VDF;
using SProject.VDF.Extensions;
using SteamTools.Common;
using SteamTools.ProfileScanner.Abstractions;
using SteamTools.ProfileScanner.Models.ScanningResults;

namespace SteamTools.ProfileScanner.Services.Scanners;

internal sealed class AppmanifestScanner(SteamClient steamClient) : IScanner
{
    public IEnumerable<ResultBase> EnumerateProfiles()
    {
        if (steamClient.Steam is null) yield break;

        foreach (var steamLibrary in steamClient.Steam.GetSteamLibraries())
        {
            var steamapps = steamLibrary.GetSteamappsDirectory();
            if (steamapps is null) continue;

            foreach (var file in steamapps.EnumerateFiles())
            {
                var appstate = ValveDataFileParser.Parse(file).PrimarySection;
                if (appstate?.Properties.AsInt64("LastOwner", out var id64) is not true) continue;

                var profile = new SteamProfile(id64);
                yield return new AppmanifestResult(profile)
                    { Name = appstate.Properties.FirstOrDefault("name")?.Value };
            }
        }
    }
}