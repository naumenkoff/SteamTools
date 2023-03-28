using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using SteamTools.LocalProfileScanner.Clients;
using SteamTools.LocalProfileScanner.Helpers;
using SteamTools.LocalProfileScanner.Models;

namespace SteamTools.LocalProfileScanner.AccountEntries;

public partial class AppmanifestEntry : ISteamID, IAppmanifestEntry
{
    private AppmanifestEntry(FileSystemInfo appmanifest, Match match)
    {
        File = appmanifest;
        Name = match.Groups[2].Value;
        Steam64 = long.Parse(match.Groups[3].Value);
        Steam32 = SteamIDConverter.ConvertSteamID64ToSteamID32(Steam64);
    }

    public FileSystemInfo File { get; }
    public string Name { get; }
    public long Steam32 { get; }
    public long Steam64 { get; }

    public static async Task<IEnumerable<ISteamID>> FindAccountsAsync()
    {
        if (SteamClient.SteamLibraries is null) return Enumerable.Empty<ISteamID>();
        var matches = new ConcurrentBag<ISteamID>();

        foreach (var steamLibrary in SteamClient.SteamLibraries)
        {
            var steamapps = SteamClient.GetSteamappsDirectory(steamLibrary);
            if (LocationRecipient.FileSystemInfoExists(steamapps) is false) continue;
            var files = steamapps.GetFiles();
            await Parallel.ForEachAsync(files, async (file, _) =>
            {
                var content = await LocationRecipient.ReadFileContentAsync(file);
                if (string.IsNullOrEmpty(content)) return;

                var match = Pattern().Match(content);
                if (match.Success is false) return;

                var account = new AppmanifestEntry(file, match);
                matches.Add(account);
            });
        }

        return matches;
    }

    [GeneratedRegex(@"""appid""\s+""(\d+)""[\s\S]+?""name""\s+""([^""]+)""[\s\S]+?""LastOwner""\s+""(\d+)""")]
    private static partial Regex Pattern();
}

public interface IAppmanifestEntry
{
    FileSystemInfo File { get; }
    string Name { get; }
}