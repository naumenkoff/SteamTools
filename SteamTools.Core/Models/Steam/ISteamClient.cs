namespace SteamTools.Core.Models.Steam;

public interface ISteamClient
{
    FileInfo ConfigFile { get; }
    FileInfo LoginusersFile { get; }
    List<DirectoryInfo> SteamLibraries { get; }
    DirectoryInfo UserdataDirectory { get; }
    DirectoryInfo GetSteamappsDirectory(FileSystemInfo steamLibraryPath);
    DirectoryInfo GetWorkshopDirectory(FileSystemInfo steamappsDirectory);
}