namespace SteamTools.Domain.Models.LScanning;

public class ConfigData : ISteamIDPair
{
    public ConfigData(string login, ISteamIDPair steamIDPair)
    {
        Login = login;
        ID32 = steamIDPair.ID32;
        ID64 = steamIDPair.ID64;
    }

    public string Login { get; }
    public SteamID32 ID32 { get; }
    public SteamID64 ID64 { get; }
}