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
    
    public IEnumerable<FileInfo> GetUniqueExtensions()
    {
        return Steam is not null
            ? Steam.GetSteamLibraries()
                .SelectMany(x => x.WorkingDirectory.EnumerateFiles("*.*", SearchOption.AllDirectories))
                .DistinctBy(file => file.Extension, StringComparer.OrdinalIgnoreCase).OrderBy(x => x.Extension.Length)
            : [];
    }
}