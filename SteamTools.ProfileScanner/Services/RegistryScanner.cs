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

        var results = new List<LocalResult>();
        foreach (var subKey in registryKey.GetSubKeyNames())
        {
            if (!uint.TryParse(subKey, out var id32)) continue;

            var profile = new SteamProfile(id32);
            var result = new LocalResult(profile, LocalResultType.Registry);
            results.Add(result);
        }

        return results;
    }
}