using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Models.ProfileData;

public class ConfigData : ISteamID
{
    public ConfigData(string login, SteamID64 steamID64)
    {
        Login = login;
        Steam64 = steamID64;
        Steam32 = steamID64.ToSteamID32();
    }

    public string Login { get; }
    public SteamID64 Steam64 { get; }
    public SteamID32 Steam32 { get; }
}