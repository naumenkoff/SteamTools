namespace SteamTools.Domain.Models.LScanning;

public class UserdataData : ISteamIDPair
{
    public UserdataData(ISteamIDPair steamIDPair)
    {
        ID32 = steamIDPair.ID32;
        ID64 = steamIDPair.ID64;
    }

    public SteamID32 ID32 { get; }
    public SteamID64 ID64 { get; }
}