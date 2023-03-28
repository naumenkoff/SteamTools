using SteamTools.LocalProfileScanner.AccountEntries;
using SteamTools.LocalProfileScanner.Extensions;
using SteamTools.LocalProfileScanner.Helpers;

namespace SteamTools.LocalProfileScanner.Models;

public class LocalAccount : ILocalAccount
{
    private static readonly List<LocalAccount> LocalAccounts = new();
    private readonly List<IAppmanifestEntry> _appmanifest;
    private readonly List<IAppworkshopEntry> _appworkshop;

    private LocalAccount(ISteamID steamData)
    {
        Steam32 = steamData.Steam32;
        Steam64 = steamData.Steam64;
        _appmanifest = new List<IAppmanifestEntry>();
        _appworkshop = new List<IAppworkshopEntry>();
    }

    public static IEnumerable<LocalAccount> Accounts => LocalAccounts;
    public IEnumerable<IAppmanifestEntry> Appmanifest => _appmanifest;
    public IEnumerable<IAppworkshopEntry> Appworkshop => _appworkshop;
    public IUserdataEntry Userdata { get; private set; }
    public IConfigEntry Config { get; private set; }
    public ILoginusersEntry Loginusers { get; private set; }
    public IRegistryEntry Registry { get; private set; }
    public int DetectionsCount { get; private set; }
    public long Steam32 { get; }
    public long Steam64 { get; }

    public string GetLogin()
    {
        if (string.IsNullOrEmpty(Config?.Login) is false) return Config.Login;
        return string.IsNullOrEmpty(Loginusers?.Login) is false
            ? Loginusers.Login
            : string.Empty;
    }

    public static LocalAccount GetAccount(ISteamID steamData)
    {
        var account = Accounts.FirstOrDefault(x => x.IsMatch(steamData));
        if (account is not null) return account;

        account = new LocalAccount(steamData);
        LocalAccounts.Add(account);
        return account;
    }

    public void Attach(ISteamID account)
    {
        if (SteamIDConverter.IsSteam64(account.Steam64) is false) return;

        var type = account.GetType();

        if (type == typeof(ConfigEntry)) Config = account as IConfigEntry;
        else if (type == typeof(LoginusersEntry)) Loginusers = account as ILoginusersEntry;
        else if (type == typeof(AppmanifestEntry)) _appmanifest.Add(account as IAppmanifestEntry);
        else if (type == typeof(AppworkshopEntry)) _appworkshop.Add(account as IAppworkshopEntry);
        else if (type == typeof(UserdataEntry)) Userdata = account as IUserdataEntry;
        else if (type == typeof(RegistryEntry)) Registry = account as IRegistryEntry;

        DetectionsCount++;
    }
}