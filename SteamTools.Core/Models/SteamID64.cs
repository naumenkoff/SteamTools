using SteamTools.Core.Utilities;

namespace SteamTools.Core.Models;

public class SteamID64
{
    public SteamID64(long steamID64)
    {
        AsLong = steamID64;
        AsString = AsLong.ToString();
    }

    public SteamID64(SteamID32 steamID32)
    {
        AsLong = SteamIDConverter.ToSteamID64(steamID32.AsUInt);
        AsString = AsLong.ToString();
    }

    public long AsLong { get; }
    public string AsString { get; }

    public SteamID32 ToSteamID32()
    {
        return new SteamID32(this);
    }

    public string ToSteamPermanentUrl()
    {
        return SteamIDConverter.ToSteamPermanentUrl(AsLong);
    }
}