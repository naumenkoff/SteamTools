using System.Globalization;

namespace SteamTools.Common;

public readonly struct SteamID32
{
    public SteamID32(uint id32)
    {
        AsUInt = id32;
        AsString = id32.ToString(CultureInfo.InvariantCulture);
    }

    public uint AsUInt { get; }
    public string AsString { get; }
}