using SProject.FileSystem;
using SProject.Steam;

namespace SteamTools.Common;

public class SteamClient
{
    public SteamClient(ISteamClientFinder steamClientFinder)
    {
        Steam = steamClientFinder.FindSteamClient();
    }

    public SteamClientModel? Steam { get; }

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