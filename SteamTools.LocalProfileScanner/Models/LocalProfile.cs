using SteamTools.Core.Models;
using SteamTools.Core.Utilities;
using SteamTools.LocalProfileScanner.Extensions;
using SteamTools.LocalProfileScanner.Models.ProfileData;

namespace SteamTools.LocalProfileScanner.Models;

public class LocalProfile : ILocalProfile
{
    private static readonly List<LocalProfile> LocalAccounts = new();
    private readonly List<AppmanifestData> _appmanifest;
    private readonly List<AppworkshopData> _appworkshop;

    private LocalProfile(ISteamID steamData)
    {
        Steam32 = steamData.Steam32;
        Steam64 = steamData.Steam64;
        _appmanifest = new List<AppmanifestData>();
        _appworkshop = new List<AppworkshopData>();
    }

    public static IEnumerable<LocalProfile> Accounts => LocalAccounts;
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

    public static LocalProfile GetAccount(ISteamID steamData)
    {
        var account = Accounts.FirstOrDefault(x => x.IsMatch(steamData));
        if (account is not null) return account;

        account = new LocalProfile(steamData);
        LocalAccounts.Add(account);
        return account;
    }

    public void Attach(ISteamID account)
    {
        if (SteamIDValidator.IsSteamID64(account.Steam64) is false) return;

        var type = account.GetType();

        if (type == typeof(ConfigData)) Config = account as ConfigData;
        else if (type == typeof(LoginusersData)) Loginusers = account as LoginusersData;
        else if (type == typeof(AppmanifestData)) _appmanifest.Add(account as AppmanifestData);
        else if (type == typeof(AppworkshopData)) _appworkshop.Add(account as AppworkshopData);
        else if (type == typeof(UserdataData)) Userdata = account as UserdataData;
        else if (type == typeof(RegistryData)) Registry = account as RegistryData;

        DetectionsCount++;
    }
}