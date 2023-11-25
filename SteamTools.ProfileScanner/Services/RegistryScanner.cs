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
        if (registryKey is null) return Enumerable.Empty<LocalResult>();

        return from id32 in registryKey.GetSubKeyNames().Select(uint.Parse)
            let profile = new SteamProfile(id32)
            select new LocalResult(profile, LocalResultType.Registry);
    }
}