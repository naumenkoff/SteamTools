namespace SteamTools.Core.Models.Steam;

public interface ISteamDirectoryFinder
{
    DirectoryInfo GetSteamDirectory();
}