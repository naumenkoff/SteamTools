using SProject.FileSystem;
using SteamTools.Domain.Models;
using SteamTools.Infrastructure.Models;
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
        return _steamClient.Steam?.GetUserdataDirectory()?.EnumerateDirectoriesAs(directory => new SteamProfile(uint.Parse(directory.Name)))
            .Select(steamProfile => new LocalResult(steamProfile, LocalResultType.Userdata)) ?? Enumerable.Empty<LocalResult>();
    }
}