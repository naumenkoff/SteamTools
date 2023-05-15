using Microsoft.Win32;
using SteamTools.Core.Utilities;

namespace SteamTools.Core.Models.Steam;

public class SteamDirectoryFinder : ISteamDirectoryFinder
{
    private const string SteamRegistryKeyPath = @"SOFTWARE\WOW6432Node\Valve\Steam";
    private const string InstallPathValueName = "InstallPath";

    public DirectoryInfo GetSteamDirectory()
    {
        var installPath = GetPathFromRegistry();
        return FileSystemHelper.GetDirectory(installPath);
    }

    private static string GetPathFromRegistry()
    {
        using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        using var steamKey = baseKey.OpenSubKey(SteamRegistryKeyPath);
        var installPath = steamKey?.GetValue(InstallPathValueName);
        return installPath?.ToString();
    }
}