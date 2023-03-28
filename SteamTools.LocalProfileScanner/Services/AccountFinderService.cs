using SteamTools.LocalProfileScanner.AccountEntries;
using SteamTools.LocalProfileScanner.Models;

namespace SteamTools.LocalProfileScanner.Services;

public static class AccountFinderService
{
    public static async Task FindAccountsAsync()
    {
        var result = await Task.WhenAll(
            ConfigEntry.FindAccountsAsync(),
            LoginusersEntry.FindAccountsAsync(),
            RegistryEntry.FindAccountsAsync(),
            UserdataEntry.FindAccountsAsync(),
            AppmanifestEntry.FindAccountsAsync(),
            AppworkshopEntry.FindAccountsAsync());

        foreach (var x in result)
        foreach (var y in x)
        {
            var localAccount = LocalAccount.GetAccount(y);
            localAccount.Attach(y);
        }
    }
}