using SteamTools.LocalProfileScanner.Models;

namespace SteamTools.LocalProfileScanner.Extensions;

public static class SteamIDExtension
{
    public static bool IsMatch(this ISteamID first, ISteamID second)
    {
        return first.Steam32.AsUInt == second.Steam32.AsUInt && first.Steam64.AsLong == second.Steam64.AsLong;
    }
}