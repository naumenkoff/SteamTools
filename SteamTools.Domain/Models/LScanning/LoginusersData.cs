namespace SteamTools.Domain.Models.LScanning;

public class LoginusersData : ISteamIDPair
{
    public LoginusersData(string login, string name, DateTimeOffset timestamp, ISteamIDPair steamIDPair)
    {
        Login = login;
        Name = name;
        Timestamp = timestamp;
        ID32 = steamIDPair.ID32;
        ID64 = steamIDPair.ID64;
    }

    public string Login { get; }
    public string Name { get; }
    public DateTimeOffset Timestamp { get; }
    public SteamID32 ID32 { get; }
    public SteamID64 ID64 { get; }
}