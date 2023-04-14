using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Models.ProfileData;

public class AppmanifestData : ISteamID
{
    public AppmanifestData(string name, SteamID64 steam64)
    {
        Name = name;
        Steam64 = steam64;
        Steam32 = steam64.ToSteamID32();
    }

    public string Name { get; }
    public SteamID64 Steam64 { get; }
    public SteamID32 Steam32 { get; }
}