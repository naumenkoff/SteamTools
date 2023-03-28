using SteamTools.LocalProfileScanner.Clients;
using SteamTools.LocalProfileScanner.Helpers;
using SteamTools.LocalProfileScanner.Models;

#pragma warning disable CS1998

namespace SteamTools.LocalProfileScanner.AccountEntries;

public class UserdataEntry : ISteamID, IUserdataEntry
{
    private UserdataEntry(FileSystemInfo directory)
    {
        Steam32 = long.Parse(directory.Name);
        Steam64 = SteamIDConverter.ConvertSteamID32ToSteamID64(Steam32);
        Directory = directory;
    }

    public long Steam32 { get; }
    public long Steam64 { get; }
    public FileSystemInfo Directory { get; }

    public static async Task<IEnumerable<ISteamID>> FindAccountsAsync()
    {
        if (LocationRecipient.FileSystemInfoExists(SteamClient.UserdataDirectory) is false)
            return Enumerable.Empty<ISteamID>();
        var directories = SteamClient.UserdataDirectory.GetDirectories();
        return directories.Select(x => new UserdataEntry(x));
    }
}

public interface IUserdataEntry
{
    FileSystemInfo Directory { get; }
}