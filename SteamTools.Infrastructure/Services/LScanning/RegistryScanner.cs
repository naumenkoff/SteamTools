using Microsoft.Win32;
using SteamTools.Domain.Models;
using SteamTools.Domain.Models.LScanning;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services.LScanning;

public class RegistryScanner : IScanner
{
    private const string RegistrySteamUsersPath = @"Software\Valve\Steam\Users";

    public IEnumerable<ISteamIDPair> GetProfiles()
    {
        using var registryKey = Registry.CurrentUser.OpenSubKey(RegistrySteamUsersPath);
        return registryKey?.GetSubKeyNames().Select(user => new SteamProfile(uint.Parse(user)))
            .Select(steamProfile => new RegistryData(steamProfile)) ?? Enumerable.Empty<RegistryData>();
    }
}