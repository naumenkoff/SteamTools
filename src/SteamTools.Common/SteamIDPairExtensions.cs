namespace SteamTools.Common;

public static class SteamIDPairExtensions
{
    public static bool IsMatch(this ISteamIDPair first, ISteamIDPair second)
    {
        return first.ID32.AsUInt == second.ID32.AsUInt && first.ID64.AsLong == second.ID64.AsLong;
    }
}