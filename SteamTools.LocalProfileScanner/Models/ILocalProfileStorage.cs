using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Models;

public interface ILocalProfileStorage
{
    IEnumerable<ILocalProfile> Accounts { get; }
    ILocalProfile GetAccount(ISteamIDPair steamIDPair);
}