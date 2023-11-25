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

        var userdata = _steamClient.Steam.GetUserdataDirectory();
        if (userdata is null) return Enumerable.Empty<LocalResult>();

        var results = new List<LocalResult>();
        foreach (var directory in userdata.EnumerateDirectories())
        {
            if (!uint.TryParse(directory.Name, out var id32)) continue;

            var profile = new SteamProfile(id32);
            var result = new LocalResult(profile, LocalResultType.Userdata);
            results.Add(result);
        }

        return results;
    }
}