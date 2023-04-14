using Microsoft.Win32;
using SteamTools.Core.Models;
using SteamTools.LocalProfileScanner.Models.ProfileData;

namespace SteamTools.LocalProfileScanner.Models.DataScanner;

public class RegistryScanner : IScanner
{
    public IEnumerable<ISteamID> GetProfiles()
    {
        using var registryKey = Registry.CurrentUser.OpenSubKey("Software")?.OpenSubKey("Valve")?.OpenSubKey("Steam")
            ?.OpenSubKey("Users");
        return registryKey?.GetSubKeyNames().Select(user => new SteamID32(uint.Parse(user)))
            .Select(steamID32 => new RegistryData(steamID32)).ToList() ?? Enumerable.Empty<RegistryData>();
    }
}