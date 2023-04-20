using Microsoft.Win32;
using SteamTools.Core.Utilities;

namespace SteamTools.Core.Models.Steam;

public class SteamDirectoryFinder : ISteamDirectoryFinder
{
    public DirectoryInfo FindSteamDirectory()
    {
        var installPath = GetPathFromRegistry();
        var steamDirectory = FileSystemHelper.GetDirectory(installPath);
        if (steamDirectory is null) throw new NotImplementedException();

        return steamDirectory;
    }

    public bool IsSteamDirectoryValid(DirectoryInfo directoryInfo)
    {
        if (directoryInfo is null) return false;

        var files = directoryInfo.EnumerateFiles();
        return files.Any(x => x.Name is "steam.exe");
    }

    private static string GetPathFromRegistry()
    {
        using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        using var steamKey = baseKey.OpenSubKey("SOFTWARE\\WOW6432Node\\Valve\\Steam");
        var installPath = steamKey?.GetValue("InstallPath");
        return installPath?.ToString();
    }
}