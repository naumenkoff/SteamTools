using SProject.VDF;
using SteamTools.Common;

namespace SteamTools.ProfileScanner;

internal sealed class AppworkshopScanner(SteamClient steamClient) : IScanner
{
    public IEnumerable<LocalResult> GetProfiles()
    {
        if (steamClient.Steam is null) yield break;

        foreach (var file in steamClient.Steam.GetSteamLibraries().Select(x => x.GetSteamappsDirectory()).Select(SteamClient.GetWorkshopDirectory)
                     .OfType<DirectoryInfo>().SelectMany(x => x.EnumerateFiles()))
        {
            var appworkshop = VParser.Parse(file);
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