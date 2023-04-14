namespace SteamTools.Core.Utilities;

public static class SteamIDValidator
{
    public static bool IsSteamID64(string id)
    {
        return long.TryParse(id, out var steamID64) && IsSteamID64(steamID64);
    }

    public static bool IsSteamID64(long id)
    {
        return id is >= SteamIDConstants.SteamID64Offset and <= SteamIDConstants.SteamID64MaximumValue;
    }

    public static bool IsSteamID32(string id)
    {
        return uint.TryParse(id, out var steamID32) && IsSteamID32(steamID32);
    }

    public static bool IsSteamID32(uint steamID32)
    {
        return steamID32 != 0;
    }
}