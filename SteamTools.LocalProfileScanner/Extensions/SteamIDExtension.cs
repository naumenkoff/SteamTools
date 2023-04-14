using SteamTools.LocalProfileScanner.Models;

namespace SteamTools.LocalProfileScanner.Extensions;

public static class SteamIDExtension
{
    public static bool IsMatch(this ISteamID first, ISteamID second)
    {
        return first.Steam32.ID32 == second.Steam32.ID32 && first.Steam64.ID64 == second.Steam64.ID64;
    }
}