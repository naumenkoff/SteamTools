using SteamTools.Core.Utilities;

namespace SteamTools.Core.Models;

public class SteamID32
{
    private readonly string _asString;

    public SteamID32(uint steamID32)
    {
        AsUInt = steamID32;
        _asString = AsUInt.ToString();
    }

    public SteamID32(SteamID64 steamID64)
    {
        AsUInt = SteamIDConverter.ToSteamID32(steamID64);
        _asString = AsUInt.ToString();
    }

    public uint AsUInt { get; }

    public override string ToString()
    {
        return _asString;
    }

    public static implicit operator string(SteamID32 steamID32)
    {
        return steamID32._asString;
    }

    public static implicit operator uint(SteamID32 steamID32)
    {
        return steamID32.AsUInt;
    }

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