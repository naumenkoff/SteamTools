namespace SteamTools.Domain.Models.LScanning;

public class AppworkshopData : ISteamIDPair
{
    public AppworkshopData(int appID, ISteamIDPair steamIDPair)
    {
        AppID = appID;
        ID32 = steamIDPair.ID32;
        ID64 = steamIDPair.ID64;
    }

    public int AppID { get; }
    public SteamID32 ID32 { get; }
    public SteamID64 ID64 { get; }
}