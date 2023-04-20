using SteamTools.Core.Utilities;

namespace SteamTools.Core.Models;

public class SteamID64
{
    private readonly string _id64;

    public SteamID64(long steamID64)
    {
        ID64 = steamID64;
        _id64 = ID64.ToString();
    }

    public SteamID64(SteamID32 steamID32)
    {
        ID64 = SteamIDConverter.ToSteamID64(steamID32.ID32);
        _id64 = ID64.ToString();
    }

    public long ID64 { get; }

    public override string ToString()
    {
        return _id64;
    }

    public static implicit operator string(SteamID64 steamID64)
    {
        return steamID64.ToString();
    }

    public SteamID32 ToSteamID32()
    {
        return new SteamID32(this);
    }

    public string ToSteamPermanentUrl()
    {
        return SteamIDConverter.ToSteamPermanentUrl(ID64);
    }
}