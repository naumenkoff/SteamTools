using System.Text.RegularExpressions;
using Microsoft.Win32;
using SProject.FileSystem;
using SteamTools.Domain.Models;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Models;

public partial class SteamClient : ISteamClient
{
    private const string SteamRegistryKeyPath = @"SOFTWARE\WOW6432Node\Valve\Steam";
    private const string InstallPathValueName = "InstallPath";

    private static string? GetPathFromRegistry()
    {
        using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        using var steamKey = baseKey.OpenSubKey(SteamRegistryKeyPath);
        var installPath = steamKey?.GetValue(InstallPathValueName);
        return installPath?.ToString();
    }
    
    private DirectoryInfo? GetSteamDirectory()
    {
        var installPath = GetPathFromRegistry();
        return FileSystemInfoExtensions.GetDirectoryInfo(false, installPath);
    }

    public DirectoryInfo? RootDirectory { get; }
    
    public SteamClient()
    {
        RootDirectory = GetSteamDirectory();
    }

    public DirectoryInfo? GetUserdataDirectory()
    {
        return FileSystemInfoExtensions.GetDirectoryInfo(false, RootDirectory?.FullName, "userdata");
    }

    public DirectoryInfo? GetConfigDirectory()
    {
        return FileSystemInfoExtensions.GetDirectoryInfo(false, RootDirectory?.FullName, "config");
    }
    
    public DirectoryInfo? GetSteamappsDirectory(FileSystemInfo? steamLibrary)
    {
        return FileSystemInfoExtensions.GetDirectoryInfo(false, steamLibrary?.FullName, "steamapps");
    }

    public DirectoryInfo? GetWorkshopDirectory(FileSystemInfo? steamapps)
    {
        return FileSystemInfoExtensions.GetDirectoryInfo(false, steamapps?.FullName, "workshop");
    }

    public IEnumerable<DirectoryInfo> GetSteamLibrariess(FileInfo? libraryfolders)
    {
        if (libraryfolders is null) return Enumerable.Empty<DirectoryInfo>();
        var fileContent = libraryfolders.ReadAllText();
        return string.IsNullOrEmpty(fileContent) ? Enumerable.Empty<DirectoryInfo>() : SteamLibraryPattern().Matches(fileContent).Select(x => FileSystemInfoExtensions.GetDirectoryInfo(false, x.Groups[1].Value)).Where(x => x is {Exists: true}).Select(x => x!);
    }

    [GeneratedRegex("\"path\"\\s+\"([^\"]+)\"", RegexOptions.Compiled)]
    private partial Regex SteamLibraryPattern();
}