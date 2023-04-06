using SteamTools.ProfileDataFetcher.Utilities;

namespace SteamTools.ProfileDataFetcher.Models;

public class SteamID64
{
    public SteamID64(long steamID64)
    {
        ID64 = steamID64;
    }

    public SteamID64(SteamID32 steamID32)
    {
        ID64 = SteamIDConverter.ToSteamID64(steamID32);
    }

    public long ID64 { get; }

    public static implicit operator long(SteamID64 steamID64)
    {
        return steamID64.ID64;
    }

    public static implicit operator string(SteamID64 steamID64)
    {
        return steamID64.ToString();
    }

    public override string ToString()
    {
        return ID64.ToString();
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