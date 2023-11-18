namespace SteamTools.Domain.Models.LScanning;

public class AppmanifestData : ISteamIDPair
{
    public AppmanifestData(string name, ISteamIDPair steamIDPair)
    {
        Name = name;
        ID32 = steamIDPair.ID32;
        ID64 = steamIDPair.ID64;
    }

    public string Name { get; }
    public SteamID32 ID32 { get; }
    public SteamID64 ID64 { get; }
}