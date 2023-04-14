namespace SteamTools.Core.Utilities;

/// <summary>
///     The SteamIDConverter class provides a set of methods for converting between various Steam ID formats.
/// </summary>
public static class SteamIDConverter
{
    /// <summary>
    ///     Converts a 32-bit SteamID to a 64-bit SteamID.
    /// </summary>
    /// <param name="steamID32">The 32-bit SteamID to convert.</param>
    /// <returns>The converted 64-bit SteamID.</returns>
    /// <remarks>
    ///     This method takes a 32-bit SteamID and converts it to a 64-bit SteamID.
    ///     For example, 113621430 would be converted to 76561198073887158.
    /// </remarks>
    public static long ToSteamID64(uint steamID32)
    {
        return steamID32 | SteamIDConstants.SteamID64Offset;
    }

    /// <summary>
    ///     Converts a 64-bit SteamID to a 32-bit SteamID.
    /// </summary>
    /// <param name="steamID64">The 64-bit SteamID to convert.</param>
    /// <returns>The converted 32-bit SteamID.</returns>
    /// <remarks>
    ///     This method takes a 64-bit SteamID and converts it to a 32-bit SteamID.
    ///     For example, 76561198073887158 would be converted to 113621430.
    /// </remarks>
    public static uint ToSteamID32(long steamID64)
    {
        return (uint)(steamID64 & uint.MaxValue);
    }

    /// <summary>
    ///     Returns the permanent URL for a 64-bit SteamID.
    /// </summary>
    /// <param name="steamID64">The 64-bit SteamID to get the URL for.</param>
    /// <returns>The permanent URL for the specified Steam ID.</returns>
    /// <remarks>
    ///     This method takes a 64-bit SteamID and converts it to a Permanent URL.
    ///     For example, 76561198073887158 would be converted to "https://steamcommunity.com/profiles/76561198073887158".
    /// </remarks>
    public static string ToSteamPermanentUrl(long steamID64)
    {
        return $"https://steamcommunity.com/profiles/{steamID64}";
    }

    /// <summary>
    ///     Converts a 32-bit SteamID to a Steam3ID.
    /// </summary>
    /// <param name="steamID32">The 32-bit SteamID to convert.</param>
    /// <returns>The converted Steam3ID.</returns>
    /// <remarks>
    ///     This method takes a 32-bit SteamID and converts it to a Steam3ID.
    ///     For example, 113621430 would be converted to [U:1:113621430].
    /// </remarks>
    public static string ToSteamID3(uint steamID32)
    {
        return $"[U:1:{steamID32}]";
    }

    /// <summary>
    ///     Converts a 32-bit SteamID to a SteamID.
    /// </summary>
    /// <remarks>
    ///     This method takes a 32-bit SteamID and converts it to a SteamID.
    ///     For example, 113621430 would be converted to "STEAM_0:0:56810715".
    /// </remarks>
    /// <param name="steamID32">The 32-bit SteamID to convert.</param>
    /// <returns>The converted SteamID.</returns>
    public static string ToSteamID(uint steamID32)
    {
        return $"STEAM_0:{steamID32 % 2}:{steamID32 / 2}";
    }

    /// <summary>
    ///     Converts a SteamID to a 64-bit SteamID.
    /// </summary>
    /// <remarks>
    ///     This method takes a SteamID and converts it to a 64-bit SteamID.
    ///     For example, "STEAM_0:0:56810715" would be converted to 76561198073887158.
    /// </remarks>
    /// <param name="type">The type of SteamID, either 0 or 1.</param>
    /// <param name="accountNumber">The account number.</param>
    /// <returns>The converted 64-bit SteamID.</returns>
    public static long ToSteamID64(byte type, long accountNumber)
    {
        return (accountNumber << 1) + SteamIDConstants.SteamID64Offset + type;
    }
}