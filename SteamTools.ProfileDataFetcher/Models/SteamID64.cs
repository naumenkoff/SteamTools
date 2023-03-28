using SteamTools.ProfileDataFetcher.Utilities;

namespace SteamTools.ProfileDataFetcher.Models;

public class SteamID64
{
    private readonly long _id;

    public SteamID64(long steamID64)
    {
        _id = steamID64;
    }

    public SteamID64(SteamID32 steamID32)
    {
        _id = SteamIDConverter.ToSteamID64(steamID32);
    }

    public static implicit operator long(SteamID64 steamID64)
    {
        return steamID64._id;
    }

    public static implicit operator string(SteamID64 steamID64)
    {
        return steamID64._id.ToString();
    }

    public SteamID32 ToSteamID32()
    {
        return new SteamID32(this);
    }

    public string ToSteamPermanentUrl()
    {
        return SteamIDConverter.ToSteamPermanentUrl(this);
    }
}