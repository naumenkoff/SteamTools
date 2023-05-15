using SteamTools.Core.Models;

namespace SteamTools.Core.Utilities;

public static class SteamIDPairExtension
{
    public static bool IsMatch(this ISteamIDPair first, ISteamIDPair second)
    {
        return first.Steam32.AsUInt == second.Steam32.AsUInt && first.Steam64.AsLong == second.Steam64.AsLong;
    }
}