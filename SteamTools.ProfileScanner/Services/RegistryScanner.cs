using Microsoft.Win32;
using SteamTools.Domain.Models;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner.Services;

public class RegistryScanner : IScanner
{
    private const string RegistrySteamUsersPath = @"Software\Valve\Steam\Users";

    public IEnumerable<LocalResult> GetProfiles()
    {
        using var registryKey = Registry.CurrentUser.OpenSubKey(RegistrySteamUsersPath);
        return registryKey?.GetSubKeyNames().Select(user => new SteamProfile(uint.Parse(user)))
            .Select(steamProfile => new LocalResult(steamProfile, LocalResultType.Registry)) ?? Enumerable.Empty<LocalResult>();
    }
}