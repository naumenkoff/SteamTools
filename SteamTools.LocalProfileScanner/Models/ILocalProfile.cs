using SteamTools.LocalProfileScanner.Models.ProfileData;

namespace SteamTools.LocalProfileScanner.Models;

public interface ILocalProfile : ISteamID
{
    IEnumerable<AppmanifestData> Appmanifest { get; }
    IEnumerable<AppworkshopData> Appworkshop { get; }
    UserdataData Userdata { get; }
    ConfigData Config { get; }
    LoginusersData Loginusers { get; }
    RegistryData Registry { get; }
    int DetectionsCount { get; }
    public string GetLogin();
}