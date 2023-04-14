namespace SteamTools.Core.Utilities;

public static class ByteConverter
{
    public static long ConvertFromMegabytes(long megabytes)
    {
        return megabytes * 1024 * 1024;
    }
}