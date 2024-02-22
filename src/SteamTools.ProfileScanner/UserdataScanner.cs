using SProject.FileSystem;
using SteamTools.Common;

namespace SteamTools.ProfileScanner;

internal sealed class UserdataScanner(SteamClient steamClient) : IScanner
{
    public IEnumerable<LocalResult> GetProfiles()
    {
        var userdata = steamClient.Steam?.GetUserdataDirectory();
        if (userdata is null) yield break;

        foreach (var id32 in userdata.EnumerateDirectoriesAs(x => uint.Parse(x.Name)))
        {
            var profile = new SteamProfile(id32);
            yield return new LocalResult(profile, LocalResultType.Userdata);
        }
    }
}