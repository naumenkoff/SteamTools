using SProject.FileSystem;
using SteamTools.Domain.Models;
using SteamTools.Domain.Services;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner.Services;

public class UserdataScanner : IScanner
{
    private readonly SteamClient _steamClient;

    public UserdataScanner(SteamClient steamClient)
    {
        _steamClient = steamClient;
    }

    public IEnumerable<LocalResult> GetProfiles()
    {
        var userdata = _steamClient.Steam?.GetUserdataDirectory();
        if (userdata is null) yield break;
        
        foreach (var id32 in userdata.EnumerateDirectoriesAs(x => uint.Parse(x.Name)))
        {
            var profile = new SteamProfile(id32);
            yield return new LocalResult(profile, LocalResultType.Userdata);
        }
    }
}