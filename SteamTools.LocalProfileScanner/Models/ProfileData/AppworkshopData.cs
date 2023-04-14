using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Models.ProfileData;

public class AppworkshopData : ISteamID
{
    public AppworkshopData(int appID, SteamID32 steam32)
    {
        AppID = appID;
        Steam32 = steam32;
        Steam64 = steam32.ToSteamID64();
    }

    public int AppID { get; }
    public SteamID64 Steam64 { get; }
    public SteamID32 Steam32 { get; }
}