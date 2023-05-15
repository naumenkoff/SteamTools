using System.Text.RegularExpressions;
using SteamTools.Core.Utilities;

namespace SteamTools.Core.Models.Steam;

public partial class SteamClient : ISteamClient
{
    public SteamClient(ISteamDirectoryFinder steamDirectoryFinder)
    {
        var steamDirectory = steamDirectoryFinder.GetSteamDirectory();
        var configDirectory = FileSystemHelper.GetDirectory(steamDirectory?.FullName, "config");

        var steamappsDirectory = GetSteamappsDirectory(steamDirectory);
        var libraryfoldersFile = FileSystemHelper.GetFile(steamappsDirectory?.FullName, "libraryfolders.vdf");

        LoginusersFile = FileSystemHelper.GetFile(configDirectory?.FullName, "loginusers.vdf");
        ConfigFile = FileSystemHelper.GetFile(configDirectory?.FullName, "config.vdf");

        UserdataDirectory = FileSystemHelper.GetDirectory(steamDirectory?.FullName, "userdata");
        SteamLibraries = GetSteamLibrariess(libraryfoldersFile).ToList();
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

    private static IEnumerable<DirectoryInfo> GetSteamLibrariess(FileInfo libraryfolders)
    {
        if (libraryfolders is null) return Enumerable.Empty<DirectoryInfo>();

        var fileContent = FileSystemHelper.ReadAllText(libraryfolders);
        return string.IsNullOrEmpty(fileContent)
            ? Enumerable.Empty<DirectoryInfo>()
            : SteamLibraryPattern().Matches(fileContent).Select(x => FileSystemHelper.GetDirectory(x.Groups[1].Value))
                .Where(x => x is not null);
    }

    [GeneratedRegex("\"path\"\\s+\"([^\"]+)\"", RegexOptions.Compiled)]
    private static partial Regex SteamLibraryPattern();
}