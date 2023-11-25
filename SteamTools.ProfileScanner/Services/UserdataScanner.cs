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
        if (_steamClient.Steam is null) return Enumerable.Empty<LocalResult>();

        return from id32 in _steamClient.Steam.GetUserdataDirectory()?.EnumerateDirectories().Select(x => uint.Parse(x.Name))
            let profile = new SteamProfile(id32)
            select new LocalResult(profile, LocalResultType.Userdata);
    }
}