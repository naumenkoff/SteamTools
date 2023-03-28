using Microsoft.Win32;
using SteamTools.LocalProfileScanner.Helpers;
using SteamTools.LocalProfileScanner.Models;

#pragma warning disable CS1998

namespace SteamTools.LocalProfileScanner.AccountEntries;

public class RegistryEntry : ISteamID, IRegistryEntry
{
    private RegistryEntry(string registryPath, string id)
    {
        Steam32 = long.Parse(id);
        Steam64 = SteamIDConverter.ConvertSteamID32ToSteamID64(Steam32);
        RegistryKey = registryPath;
    }

    public string RegistryKey { get; }
    public long Steam32 { get; }
    public long Steam64 { get; }

    public static async Task<IEnumerable<ISteamID>> FindAccountsAsync()
    {
        using var registryKey = Registry.CurrentUser.OpenSubKey("Software")?.OpenSubKey("Valve")?.OpenSubKey("Steam")
            ?.OpenSubKey("Users");
        if (registryKey is null) return Enumerable.Empty<ISteamID>();
        var users = registryKey.GetSubKeyNames();
        return (from user in users let path = Path.Combine(registryKey.Name, user) select new RegistryEntry(path, user))
            .ToList();
    }
}

public interface IRegistryEntry
{
    string RegistryKey { get; }
}