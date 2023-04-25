using SteamTools.Core.Utilities;

namespace SteamTools.Core.Models;

public class SteamID64
{
    private readonly string _asString;

    public SteamID64(long steamID64)
    {
        AsLong = steamID64;
        _asString = AsLong.ToString();
    }

    public SteamID64(SteamID32 steamID32)
    {
        AsLong = SteamIDConverter.ToSteamID64(steamID32);
        _asString = AsLong.ToString();
    }

    public long AsLong { get; }

    public override string ToString()
    {
        return _asString;
    }

    public static implicit operator string(SteamID64 steamID64)
    {
        return steamID64._asString;
    }

    public static implicit operator long(SteamID64 steamID64)
    {
        return steamID64.AsLong;
    }

    public SteamID32 ToSteamID32()
    {
        return new SteamID32(this);
    }

    public string ToSteamPermanentUrl()
    {
        return SteamIDConverter.ToSteamPermanentUrl(AsLong);
    }
}