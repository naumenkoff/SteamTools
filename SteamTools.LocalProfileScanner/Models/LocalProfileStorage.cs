using SteamTools.Core.Models;
using SteamTools.Core.Utilities;

namespace SteamTools.LocalProfileScanner.Models;

public class LocalProfileStorage : ILocalProfileStorage
{
    private readonly List<ILocalProfile> _accounts;

    public LocalProfileStorage()
    {
        _accounts = new List<ILocalProfile>();
    }

    public IEnumerable<ILocalProfile> Accounts => _accounts;

    // update account or smth like that
    public ILocalProfile GetAccount(ISteamIDPair steamData)
    {
        var account = Accounts.FirstOrDefault(x => x.IsMatch(steamData));
        if (account is not null) return account;

        account = new LocalProfile(steamData);
        _accounts.Add(account);
        return account;
    }
}