using System.Text.RegularExpressions;
using Microsoft.Win32;
using SteamTools.LocalProfileScanner.Helpers;

namespace SteamTools.LocalProfileScanner.Clients;

public static partial class SteamClient
{
    public static FileInfo ConfigFile { get; private set; }
    public static FileInfo LoginusersFile { get; private set; }
    public static DirectoryInfo[] SteamLibraries { get; private set; }
    public static DirectoryInfo UserdataDirectory { get; private set; }

    public static async Task InitializeAsync()
    {
        var installationDirectory = GetGenuineInstallationPath();
        if (installationDirectory is null) return;

        UserdataDirectory = LocationRecipient.GetDirectory(installationDirectory.FullName, "userdata");

        var steamappsDirectory = GetSteamappsDirectory(installationDirectory);
        if (LocationRecipient.FileSystemInfoExists(steamappsDirectory))
        {
            var libraryfoldersFile = LocationRecipient.GetFile(steamappsDirectory.FullName, "libraryfolders.vdf");
            var steamLibraries = await GetSteamLibrariesAsync(libraryfoldersFile);
            SteamLibraries = steamLibraries?.ToArray();
        }

        var configDirectory = LocationRecipient.GetDirectory(installationDirectory.FullName, "config");
        if (LocationRecipient.FileSystemInfoExists(configDirectory))
        {
            LoginusersFile = LocationRecipient.GetFile(configDirectory.FullName, "loginusers.vdf");
            ConfigFile = LocationRecipient.GetFile(configDirectory.FullName, "config.vdf");
        }
    }

    private static DirectoryInfo GetGenuineInstallationPath()
    {
        var registryPath = GetInstallPathFromRegistry();
        if (IsClientGenuine(registryPath)) return registryPath;

        return null;
    }

    private static DirectoryInfo GetInstallPathFromRegistry()
    {
        using var steam = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
            .OpenSubKey("SOFTWARE")?.OpenSubKey("WOW6432Node")?.OpenSubKey("Valve")?.OpenSubKey("Steam");
        var path = steam?.GetValue("InstallPath")?.ToString();
        return LocationRecipient.GetDirectory(path);
    }

    private static bool IsClientGenuine(DirectoryInfo directory)
    {
        if (LocationRecipient.FileSystemInfoExists(directory) is false) return false;
        var steamFiles = directory.GetFiles();
        return steamFiles.Count(x => x.Name is "steam.exe" or "Steam.dll") == 2;
    }

    public static DirectoryInfo GetSteamappsDirectory(FileSystemInfo steamLibraryPath)
    {
        return LocationRecipient.GetDirectory(steamLibraryPath?.FullName, "steamapps");
    }

    public static DirectoryInfo GetWorkshopDirectory(FileSystemInfo steamappsDirectory)
    {
        return LocationRecipient.GetDirectory(steamappsDirectory?.FullName, "workshop");
    }

    private static async Task<IEnumerable<DirectoryInfo>> GetSteamLibrariesAsync(FileInfo libraryfoldersFile)
    {
        var fileContent = await LocationRecipient.ReadFileContentAsync(libraryfoldersFile);
        if (string.IsNullOrEmpty(fileContent)) return Enumerable.Empty<DirectoryInfo>();
        var libraries = SteamLibrariesPattern().Matches(fileContent).Select(x => x.Groups[1].Value);
        return libraries.Select(x => new DirectoryInfo(x)).Where(x => x.Exists);
    }

    [GeneratedRegex(@"""path""\s*""([^""]*)""")]
    private static partial Regex SteamLibrariesPattern();
}