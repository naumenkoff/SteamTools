using SProject.FileSystem;
using SteamTools.Domain.Models;
using SteamTools.Domain.Models.LScanning;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services.LScanning;

public class UserdataScanner : IScanner
{
    private readonly ISteamClient _steamClient;

    public UserdataScanner(ISteamClient steamClient)
    {
        _steamClient = steamClient;
    }

    public IEnumerable<ISteamIDPair> GetProfiles()
    {
        return _steamClient.UserdataDirectory?.EnumerateDirectoriesAs(directory => new SteamProfile(uint.Parse(directory.Name)))
            .Select(steamProfile => new UserdataData(steamProfile)) ?? Enumerable.Empty<UserdataData>();
    }
}