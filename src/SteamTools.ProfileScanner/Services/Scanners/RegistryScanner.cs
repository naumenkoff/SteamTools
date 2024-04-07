using Microsoft.Win32;
using SteamTools.Common;
using SteamTools.ProfileScanner.Abstractions;
using SteamTools.ProfileScanner.Enums;
using SteamTools.ProfileScanner.Models.ScanningResults;

namespace SteamTools.ProfileScanner.Services.Scanners;

internal sealed class RegistryScanner : IScanner
{
    private const string RegistrySteamUsersPath = @"Software\Valve\Steam\Users";

    public IEnumerable<ResultBase> EnumerateProfiles()
    {
        using var registryKey = Registry.CurrentUser.OpenSubKey(RegistrySteamUsersPath);
        if (registryKey is null) yield break;

        foreach (var profile in EnumerateSteamIDs(registryKey).Select(x => new SteamProfile(x)))
            yield return new ResultBase(profile, ResultType.Registry);
    }

    private static IEnumerable<uint> EnumerateSteamIDs(RegistryKey parentRegistryKey)
    {
        foreach (var childRegistryKey in parentRegistryKey.GetSubKeyNames())
            if (uint.TryParse(childRegistryKey, out var id))
                yield return id;
    }
}