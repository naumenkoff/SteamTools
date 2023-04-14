using System.Text.RegularExpressions;
using Microsoft.Win32;
using SteamTools.Core.Utilities;

namespace SteamTools.Core.Models;

public partial class SteamClient : ISteamClient
{
    public SteamClient()
    {
        var steamDirectory = FindSteamDirectory();
        UserdataDirectory = FileSystemHelper.GetDirectory(steamDirectory.FullName, "userdata");

        var steamappsDirectory = GetSteamappsDirectory(steamDirectory);
        if (steamappsDirectory is not null)
        {
            var libraryfoldersFile = FileSystemHelper.GetFile(steamappsDirectory.FullName, "libraryfolders.vdf");
            SteamLibraries = GetSteamLibraries(libraryfoldersFile).ToList();
        }

        var configDirectory = FileSystemHelper.GetDirectory(steamDirectory.FullName, "config");
        if (configDirectory is null) return;
        LoginusersFile = FileSystemHelper.GetFile(configDirectory.FullName, "loginusers.vdf");
        ConfigFile = FileSystemHelper.GetFile(configDirectory.FullName, "config.vdf");
    }

    public Task<HashSet<string>> GetExtensionsAsync()
    {
        var hashSet = new HashSet<string>();
        foreach (var file in SteamLibraries
                     .Select(directory => directory.GetFiles("*.*", SearchOption.AllDirectories))
                     .SelectMany(files => files)) hashSet.Add(file.Extension);
        return Task.FromResult(hashSet);
    }

    public FileInfo ConfigFile { get; }
    public FileInfo LoginusersFile { get; }
    public List<DirectoryInfo> SteamLibraries { get; }
    public DirectoryInfo UserdataDirectory { get; }

    public DirectoryInfo GetSteamappsDirectory(FileSystemInfo steamLibraryPath)
    {
        return FileSystemHelper.GetDirectory(steamLibraryPath?.FullName, "steamapps");
    }

    public DirectoryInfo GetWorkshopDirectory(FileSystemInfo steamappsDirectory)
    {
        return FileSystemHelper.GetDirectory(steamappsDirectory?.FullName, "workshop");
    }

    private static DirectoryInfo FindSteamDirectory()
    {
        var registryPath = GetSteamInstallationDirectory();
        if (IsSteamDirectoryValid(registryPath)) return registryPath;
        throw new DirectoryNotFoundException("Failed to find the path to 'Steam' directory.");
    }

    private static DirectoryInfo GetSteamInstallationDirectory()
    {
        using var steam = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
            .OpenSubKey(@"SOFTWARE\WOW6432Node\Valve\Steam");
        var path = steam?.GetValue("InstallPath");
        return FileSystemHelper.GetDirectory(path?.ToString());
    }

    private static bool IsSteamDirectoryValid(DirectoryInfo directoryInfo)
    {
        if (directoryInfo is null) return false;
        var files = directoryInfo.EnumerateFiles();
        return files.Any(x => x.Name is "steam.exe");
    }

    private static IEnumerable<DirectoryInfo> GetSteamLibraries(FileInfo libraryfoldersFile)
    {
        if (libraryfoldersFile is null) return Enumerable.Empty<DirectoryInfo>();
        var fileContent = FileSystemHelper.ReadAllText(libraryfoldersFile);
        return string.IsNullOrEmpty(fileContent)
            ? Enumerable.Empty<DirectoryInfo>()
            : SteamLibraryPattern().Matches(fileContent).Select(x => FileSystemHelper.GetDirectory(x.Groups[1].Value))
                .Where(x => x is not null);
    }

    [GeneratedRegex("\"path\"\\s+\"([^\"]+)\"", RegexOptions.Compiled)]
    private static partial Regex SteamLibraryPattern();
}