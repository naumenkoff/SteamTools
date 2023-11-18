namespace SteamTools.Domain.Models;

public interface ISteamClient
{
    DirectoryInfo? RootDirectory { get; }
    DirectoryInfo? GetSteamappsDirectory(FileSystemInfo? steamLibrary);
    DirectoryInfo? GetWorkshopDirectory(FileSystemInfo? steamapps);
    IEnumerable<DirectoryInfo> GetSteamLibrariess(FileInfo? libraryfolders);
    public DirectoryInfo? GetUserdataDirectory();
    public DirectoryInfo? GetConfigDirectory();
}