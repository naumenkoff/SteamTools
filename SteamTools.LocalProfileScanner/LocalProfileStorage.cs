using SteamTools.LocalProfileScanner.Extensions;
using SteamTools.LocalProfileScanner.Models;

namespace SteamTools.LocalProfileScanner;

public class LocalProfileStorage
{
    private readonly List<ILocalProfile> _accounts;

    public LocalProfileStorage()
    {
        _accounts = new List<ILocalProfile>();
    }

    public IReadOnlyCollection<ILocalProfile> Accounts => _accounts;

    public ILocalProfile GetAccount(ISteamID steamData)
    {
        var account = Accounts.FirstOrDefault(x => x.IsMatch(steamData));
        if (account is not null) return account;

        account = new LocalProfile(steamData);
        _accounts.Add(account);
        return account;
    }
}