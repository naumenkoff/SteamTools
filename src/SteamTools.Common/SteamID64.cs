using System.Globalization;

namespace SteamTools.Common;

public readonly struct SteamID64
{
    public SteamID64(long id64)
    {
        AsLong = id64;
        AsString = id64.ToString(CultureInfo.InvariantCulture);
    }

    public long AsLong { get; }
    public string AsString { get; }
}