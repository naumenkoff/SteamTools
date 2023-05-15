using SteamTools.Core.Models;
using SteamTools.Core.Models.Steam;
using SteamTools.LocalProfileScanner.Models;
using SteamTools.LocalProfileScanner.Services.Interfaces;

namespace SteamTools.LocalProfileScanner.Services.Implementations;

public class UserdataScanner : IScanner
{
    private readonly ISteamClient _steamClient;

    public UserdataScanner(ISteamClient steamClient)
    {
        _steamClient = steamClient;
    }

    public IEnumerable<ISteamIDPair> GetProfiles()
    {
        if (_steamClient.UserdataDirectory is null) return Enumerable.Empty<UserdataData>();
        var directories = _steamClient.UserdataDirectory.GetDirectories();
        return directories.Select(directory => new SteamID32(uint.Parse(directory.Name)))
            .Select(steamID32 => new UserdataData(steamID32.ToSteamID64(), steamID32));
    }
}