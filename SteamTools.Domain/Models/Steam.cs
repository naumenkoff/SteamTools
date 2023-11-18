using SProject.FileSystem;

namespace SteamTools.Domain.Models;

public class Steam
{
    public Steam(ISteamClient steamClient)
    {
        var configDirectory = steamClient.GetConfigDirectory();
        LoginusersFile = FileSystemInfoExtensions.GetFileInfo(false, configDirectory?.FullName, "loginusers.vdf");
        ConfigFile = FileSystemInfoExtensions.GetFileInfo(false, configDirectory?.FullName, "config.vdf");

        UserdataDirectory = steamClient.GetUserdataDirectory();
        
        var steamappsDirectory = steamClient.GetSteamappsDirectory(steamClient.RootDirectory);
        var libraryfoldersFile = FileSystemInfoExtensions.GetFileInfo(false, steamappsDirectory?.FullName, "libraryfolders.vdf");
        SteamLibraries = steamClient.GetSteamLibrariess(libraryfoldersFile).ToList();
    }
    
    public FileInfo? ConfigFile { get; }
    public FileInfo? LoginusersFile { get; }
    public List<DirectoryInfo> SteamLibraries { get; }
    public DirectoryInfo? UserdataDirectory { get; }
}