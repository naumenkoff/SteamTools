using SteamTools.ProfileDataFetcher.Utilities;

namespace SteamTools.ProfileDataFetcher.Models;

public class SteamID32
{
    private readonly uint _id;

    public SteamID32(uint steamID32)
    {
        _id = steamID32;
    }

    public SteamID32(SteamID64 steamID64)
    {
        _id = SteamIDConverter.ToSteamID32(steamID64);
    }

    public static implicit operator uint(SteamID32 steamID32)
    {
        return steamID32._id;
    }

    public static implicit operator string(SteamID32 steamID32)
    {
        return steamID32._id.ToString();
    }

    public SteamID64 ToSteamID64()
    {
        return new SteamID64(this);
    }

    public string ToSteamID3()
    {
        return SteamIDConverter.ToSteamID3(this);
    }

    public string ToSteamID()
    {
        return SteamIDConverter.ToSteamID(this);
    }
}