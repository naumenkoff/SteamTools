using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Models.ProfileData;

public class RegistryData : ISteamID
{
    public RegistryData(SteamID32 steamID32)
    {
        Steam32 = steamID32;
        Steam64 = steamID32.ToSteamID64();
    }

    public SteamID64 Steam64 { get; }
    public SteamID32 Steam32 { get; }
}