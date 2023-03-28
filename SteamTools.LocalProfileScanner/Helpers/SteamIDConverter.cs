namespace SteamTools.LocalProfileScanner.Helpers;

public static class SteamIDConverter
{
    private const long SteamID64Identifier = 0x0110000100000000;

    public static long ConvertSteamID32ToSteamID64(long steam32)
    {
        return SteamID64Identifier + steam32;
    }

    public static long ConvertSteamID64ToSteamID32(long steam64)
    {
        var steamID32 = steam64 - SteamID64Identifier;
        return steamID32;
    }

    public static bool IsSteam64(long id)
    {
        return id > SteamID64Identifier;
    }
}