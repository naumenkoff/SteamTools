using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using SteamTools.LocalProfileScanner.Clients;
using SteamTools.LocalProfileScanner.Helpers;
using SteamTools.LocalProfileScanner.Models;

namespace SteamTools.LocalProfileScanner.AccountEntries;

public partial class AppworkshopEntry : ISteamID, IAppworkshopEntry
{
    private AppworkshopEntry(FileSystemInfo appworkshop, Match match)
    {
        File = appworkshop;
        AppID = int.Parse(match.Groups[1].Value);
        Steam32 = long.Parse(match.Groups[2].Value);
        Steam64 = SteamIDConverter.ConvertSteamID32ToSteamID64(Steam32);
    }

    public int AppID { get; }
    public FileSystemInfo File { get; }
    public long Steam32 { get; }
    public long Steam64 { get; }

    public static async Task<IEnumerable<ISteamID>> FindAccountsAsync()
    {
        if (SteamClient.SteamLibraries is null) return Enumerable.Empty<ISteamID>();
        var matches = new ConcurrentBag<ISteamID>();
        foreach (var steamLibrary in SteamClient.SteamLibraries)
        {
            var steamapps = SteamClient.GetSteamappsDirectory(steamLibrary);
            var workshop = SteamClient.GetWorkshopDirectory(steamapps);
            if (LocationRecipient.FileSystemInfoExists(workshop) is false) continue;
            var files = workshop.GetFiles();
            await Parallel.ForEachAsync(files, async (file, _) =>
            {
                var content = await LocationRecipient.ReadFileContentAsync(file);
                if (string.IsNullOrEmpty(content)) return;

                var match = Pattern().Match(content);
                if (match.Success is false) return;

                var account = new AppworkshopEntry(file, match);
                matches.Add(account);
            });
        }

        return matches;
    }

    [GeneratedRegex(@"""appid""\s*""(\d+)""[\s\S]*?""subscribedby""\s*""(\d+)""")]
    private static partial Regex Pattern();
}

public interface IAppworkshopEntry
{
    FileSystemInfo File { get; }
    int AppID { get; }
}