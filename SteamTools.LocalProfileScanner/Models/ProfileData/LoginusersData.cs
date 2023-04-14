using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Models.ProfileData;

public class LoginusersData : ISteamID
{
    public LoginusersData(SteamID64 steamID64, string login, string name, long time)
    {
        Steam64 = steamID64;
        Steam32 = steamID64.ToSteamID32();
        Login = login;
        Name = name;
        Timestamp = DateTimeOffset.FromUnixTimeSeconds(time).ToLocalTime();
    }

    public string Login { get; }
    public string Name { get; }
    public DateTimeOffset Timestamp { get; }
    public SteamID64 Steam64 { get; }
    public SteamID32 Steam32 { get; }
}