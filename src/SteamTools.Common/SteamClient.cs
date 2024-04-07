using SProject.FileSystem;
using SProject.Steam;

namespace SteamTools.Common;

public class SteamClient(ISteamClientFinder steamClientFinder)
{
    public SteamClientModel? Steam { get; } = steamClientFinder.FindSteamClient();

    public FileInfo? GetLoginusersFile()
    {
        return FileSystemInfoExtensions.GetFileInfo(false, Steam?.GetConfigDirectory()?.FullName, "loginusers.vdf");
    }

    public FileInfo? GetConfigFile()
    {
        return FileSystemInfoExtensions.GetFileInfo(false, Steam?.GetConfigDirectory()?.FullName, "config.vdf");
    }

    public static DirectoryInfo? GetWorkshopDirectory(FileSystemInfo? steamapps)
    {
        return FileSystemInfoExtensions.GetDirectoryInfo(false, steamapps?.FullName, "workshop");
    }
}