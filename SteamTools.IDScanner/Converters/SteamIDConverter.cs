namespace SteamTools.IDScanner.Converters;

public static class SteamIDConverter
{
    private const long SteamID64Identifier = 0x0110000100000000;

    public static string ConvertSteamID32ToSteamID64(string steamID32)
    {
        var id = long.Parse(steamID32) + SteamID64Identifier;
        return id.ToString();
    }

    public static string ConvertSteamID64ToSteamID32(string steamID64)
    {
        var id = long.Parse(steamID64) - SteamID64Identifier;
        return id.ToString();
    }

    public static bool IsSteamID32(string text)
    {
        if (string.IsNullOrEmpty(text)) return false;
        return text.Length <= 10 && text.All(char.IsDigit);
    }

    public static bool IsSteamID64(string text)
    {
        if (string.IsNullOrEmpty(text)) return false;
        return text.Length == 17 && text.StartsWith("76561") && text.All(char.IsDigit);
    }
}