namespace SteamTools.Core.Models;

public interface ISteamClient
{
    FileInfo ConfigFile { get; }
    FileInfo LoginusersFile { get; }
    List<DirectoryInfo> SteamLibraries { get; }
    DirectoryInfo UserdataDirectory { get; }
    DirectoryInfo GetSteamappsDirectory(FileSystemInfo steamLibraryPath);
    DirectoryInfo GetWorkshopDirectory(FileSystemInfo steamappsDirectory);
    Task<HashSet<string>> GetExtensionsAsync();
}