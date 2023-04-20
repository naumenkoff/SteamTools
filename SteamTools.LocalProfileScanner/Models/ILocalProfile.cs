using SteamTools.LocalProfileScanner.Models.ProfileData;

namespace SteamTools.LocalProfileScanner.Models;

public interface ILocalProfile : ISteamID
{
    IReadOnlyCollection<AppmanifestData> Appmanifest { get; }
    IReadOnlyCollection<AppworkshopData> Appworkshop { get; }
    UserdataData Userdata { get; }
    ConfigData Config { get; }
    LoginusersData Loginusers { get; }
    RegistryData Registry { get; }
    int DetectionsCount { get; }
    public string GetLogin();
    void Attach(ISteamID account);
}