using System.Text.RegularExpressions;
using Microsoft.Win32;
using SProject.FileSystem;
using SProject.Steam.Abstractions;
using SteamTools.Domain.Models;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Models;

public class SteamClient
{
    public SteamClient(ISteamClientFinder steamClientFinder)
    {
        Steam = steamClientFinder.FindSteamClient();
    }

    public SteamClientModel? Steam { get; }

    public FileInfo? GetConfigFile()
    {
        return FileSystemInfoExtensions.GetFileInfo(false, Steam?.GetConfigDirectory()?.FullName, "loginusers.vdf");
    }

    public FileInfo? GetLoginusersFile()
    {
        return FileSystemInfoExtensions.GetFileInfo(false, Steam?.GetConfigDirectory()?.FullName, "config.vdf");
    }

    public DirectoryInfo? GetWorkshopDirectory(FileSystemInfo? steamapps)
    {
        return FileSystemInfoExtensions.GetDirectoryInfo(false, steamapps?.FullName, "workshop");
    }
}