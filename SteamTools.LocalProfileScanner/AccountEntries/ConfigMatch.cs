using System.Text.RegularExpressions;
using SteamTools.LocalProfileScanner.Clients;
using SteamTools.LocalProfileScanner.Helpers;
using SteamTools.LocalProfileScanner.Models;

namespace SteamTools.LocalProfileScanner.AccountEntries;

public partial class ConfigEntry : ISteamID, IConfigEntry
{
    private ConfigEntry(Match match)
    {
        Steam64 = long.Parse(match.Groups[2].Value);
        Steam32 = SteamIDConverter.ConvertSteamID64ToSteamID32(Steam64);
        Login = match.Groups[1].Value;
    }

    public string Login { get; }
    public long Steam32 { get; }
    public long Steam64 { get; }

    public static async Task<IEnumerable<ISteamID>> FindAccountsAsync()
    {
        var content = await LocationRecipient.ReadFileContentAsync(SteamClient.ConfigFile);
        if (string.IsNullOrEmpty(content)) return Enumerable.Empty<ISteamID>();
        var matches = Pattern().Matches(content).Cast<Match>();
        return matches.Where(x => x.Success).Select(x => new ConfigEntry(x));
    }

    [GeneratedRegex("(\\w+)\"\\s*\\{\\s*\"SteamID\"\\s*\"(\\d+)\"")]
    private static partial Regex Pattern();
}

public interface IConfigEntry
{
    string Login { get; }
}