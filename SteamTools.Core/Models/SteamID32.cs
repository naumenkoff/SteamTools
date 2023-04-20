using SteamTools.Core.Utilities;

namespace SteamTools.Core.Models;

public class SteamID32
{
    private readonly string _id32;

    public SteamID32(uint steamID32)
    {
        ID32 = steamID32;
        _id32 = ID32.ToString();
    }

    public SteamID32(SteamID64 steamID64)
    {
        ID32 = SteamIDConverter.ToSteamID32(steamID64.ID64);
        _id32 = ID32.ToString();
    }

    public uint ID32 { get; }

    public override string ToString()
    {
        return _id32;
    }

    public static implicit operator string(SteamID32 steamID32)
    {
        return steamID32.ToString();
    }

    public SteamID64 ToSteamID64()
    {
        return new SteamID64(this);
    }

    public string ToSteamID3()
    {
        return SteamIDConverter.ToSteamID3(ID32);
    }

    public string ToSteamID()
    {
        return SteamIDConverter.ToSteamID(ID32);
    }
}