namespace SteamTools.Core.Models.Steam;

public interface ISteamDirectoryFinder
{
    DirectoryInfo FindSteamDirectory();
    bool IsSteamDirectoryValid(DirectoryInfo directoryInfo);
}