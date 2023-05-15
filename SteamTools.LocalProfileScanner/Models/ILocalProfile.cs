using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Models;

public interface ILocalProfile : ISteamIDPair
{
    IEnumerable<AppmanifestData> Appmanifest { get; }
    IEnumerable<AppworkshopData> Appworkshop { get; }
    UserdataData Userdata { get; }
    ConfigData Config { get; }
    LoginusersData Loginusers { get; }
    RegistryData Registry { get; }
    int DetectionsCount { get; }
    public string GetLogin();
    void Attach(ISteamIDPair account);
}