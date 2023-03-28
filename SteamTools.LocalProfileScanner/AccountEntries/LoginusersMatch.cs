using System.Text.RegularExpressions;
using SteamTools.LocalProfileScanner.Clients;
using SteamTools.LocalProfileScanner.Helpers;
using SteamTools.LocalProfileScanner.Models;

namespace SteamTools.LocalProfileScanner.AccountEntries;

public partial class LoginusersEntry : ISteamID, ILoginusersEntry
{
    private LoginusersEntry(Match match)
    {
        Steam64 = long.Parse(match.Groups[1].Value);
        Steam32 = SteamIDConverter.ConvertSteamID64ToSteamID32(Steam64);
        Login = match.Groups[3].Captures[0].Value;
        Name = match.Groups[3].Captures[1].Value;
        var time = long.Parse(match.Groups[3].Captures[7].Value);
        Timestamp = DateTimeOffset.FromUnixTimeSeconds(time).ToLocalTime();
    }

    public string Login { get; }
    public string Name { get; }
    public DateTimeOffset Timestamp { get; }
    public long Steam32 { get; }
    public long Steam64 { get; }

    public static async Task<IEnumerable<ISteamID>> FindAccountsAsync()
    {
        var content = await LocationRecipient.ReadFileContentAsync(SteamClient.LoginusersFile);
        if (string.IsNullOrEmpty(content)) return Enumerable.Empty<ISteamID>();
        var matches = Pattern().Matches(content).Cast<Match>();
        return matches.Where(x => x.Success).Select(x => new LoginusersEntry(x));
    }

    [GeneratedRegex(@"""(\w{17})""\s*\{(?:\s*""([^""]+)""\s*""([^""]+)""\s*)+\s*\}")]
    private static partial Regex Pattern();
}

public interface ILoginusersEntry
{
    string Login { get; }
    string Name { get; }
    DateTimeOffset Timestamp { get; }
}