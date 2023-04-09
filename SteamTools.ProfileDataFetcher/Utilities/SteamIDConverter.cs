using System.Text.RegularExpressions;

namespace SteamTools.ProfileDataFetcher.Utilities;

public static class SteamIDConverter
{
    private const long SteamID64Offset = 0x0110000100000000;
    private const long SteamID32Mask = 0xFFFFFFFF;
    private const uint SteamIDMask = 0x7FFFFFFF;

    public static long ToSteamID64(uint steamID32)
    {
        return (steamID32 & SteamID32Mask) | SteamID64Offset;
    }

    public static uint ToSteamID32(long steamID64)
    {
        return (uint)(steamID64 & SteamID32Mask);
    }

    public static string ToSteamPermanentUrl(long steamID64)
    {
        return $"https://steamcommunity.com/profiles/{steamID64}";
    }

    public static string ToSteamID3(uint steamID32)
    {
        return $"[U:1:{steamID32}]";
    }

    public static string ToSteamID(uint steamID32)
    {
        var accountID = steamID32 & SteamIDMask;
        return $"STEAM_1:{accountID % 2}:{accountID / 2}";
    }

    public static long ToSteamID64(Match steamID)
    {
        var type = long.Parse(steamID.Groups[2].Value);
        var accountNumber = long.Parse(steamID.Groups[3].Value);
        return (accountNumber << 1) + SteamID64Offset + type;
    }
}