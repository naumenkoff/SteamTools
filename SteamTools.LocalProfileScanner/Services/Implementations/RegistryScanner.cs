using Microsoft.Win32;
using SteamTools.Core.Models;
using SteamTools.LocalProfileScanner.Models;
using SteamTools.LocalProfileScanner.Services.Interfaces;

namespace SteamTools.LocalProfileScanner.Services.Implementations;

public class RegistryScanner : IScanner
{
    private const string RegistrySteamUsersPath = @"Software\Valve\Steam\Users";

    public IEnumerable<ISteamIDPair> GetProfiles()
    {
        using var registryKey = Registry.CurrentUser.OpenSubKey(RegistrySteamUsersPath);
        return registryKey?.GetSubKeyNames().Select(user => new SteamID32(uint.Parse(user)))
                   .Select(steamID32 => new RegistryData(steamID32.ToSteamID64(), steamID32)) ??
               Enumerable.Empty<RegistryData>();
    }
}