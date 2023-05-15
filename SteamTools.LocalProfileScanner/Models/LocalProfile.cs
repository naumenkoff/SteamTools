using SteamTools.Core.Models;
using SteamTools.Core.Utilities;

namespace SteamTools.LocalProfileScanner.Models;

public class LocalProfile : ILocalProfile
{
    private readonly List<AppmanifestData> _appmanifest;
    private readonly List<AppworkshopData> _appworkshop;

    public LocalProfile(ISteamIDPair steamData)
    {
        Steam32 = steamData.Steam32;
        Steam64 = steamData.Steam64;
        _appmanifest = new List<AppmanifestData>();
        _appworkshop = new List<AppworkshopData>();
    }

    public IEnumerable<AppmanifestData> Appmanifest => _appmanifest;
    public IEnumerable<AppworkshopData> Appworkshop => _appworkshop;
    public UserdataData Userdata { get; private set; }
    public ConfigData Config { get; private set; }
    public LoginusersData Loginusers { get; private set; }
    public RegistryData Registry { get; private set; }
    public int DetectionsCount { get; private set; }

    public string GetLogin()
    {
        if (string.IsNullOrEmpty(Config?.Login) is false) return Config.Login;
        return string.IsNullOrEmpty(Loginusers?.Login) is false
            ? Loginusers.Login
            : string.Empty;
    }

    public SteamID32 Steam32 { get; }
    public SteamID64 Steam64 { get; }

    public void Attach(ISteamIDPair account)
    {
        if (SteamIDValidator.IsSteamID64(account.Steam64.AsLong) is false) return;

        switch (account)
        {
            case ConfigData configData:
                Config = configData;
                break;
            case LoginusersData loginusersData:
                Loginusers = loginusersData;
                break;
            case AppmanifestData appmanifestData:
                _appmanifest.Add(appmanifestData);
                break;
            case AppworkshopData appworkshopData:
                _appworkshop.Add(appworkshopData);
                break;
            case UserdataData userdataData:
                Userdata = userdataData;
                break;
            case RegistryData registryData:
                Registry = registryData;
                break;
            default:
                return;
        }

        DetectionsCount++;
    }
}