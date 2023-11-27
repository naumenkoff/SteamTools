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
        if (registryKey is null) yield break;

        foreach (var id32 in registryKey.GetSubKeyNames().Select(uint.Parse))
        {
            var profile = new SteamProfile(id32);
            yield return new LocalResult(profile, LocalResultType.Registry);
        }
    }
}