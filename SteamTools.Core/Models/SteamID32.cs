using SteamTools.Core.Utilities;

namespace SteamTools.Core.Models;

public class SteamID32
{
    public SteamID32(uint steamID32)
    {
        AsUInt = steamID32;
        AsString = AsUInt.ToString();
    }

    public SteamID32(SteamID64 steamID64)
    {
        AsUInt = SteamIDConverter.ToSteamID32(steamID64.AsLong);
        AsString = AsUInt.ToString();
    }

    public string AsString { get; }
    public uint AsUInt { get; }

    public SteamID64 ToSteamID64()
    {
        return new SteamID64(this);
    }

    public string ToSteamID3()
    {
        return SteamIDConverter.ToSteamID3(AsUInt);
    }

    public string ToSteamID()
    {
        return SteamIDConverter.ToSteamID(AsUInt);
    }
}