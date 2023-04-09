using SteamTools.ProfileDataFetcher.Utilities;

namespace SteamTools.ProfileDataFetcher.Models;

public class SteamID32
{
    public SteamID32(uint steamID32)
    {
        ID32 = steamID32;
    }

    public SteamID32(SteamID64 steamID64)
    {
        ID32 = SteamIDConverter.ToSteamID32(steamID64.ID64);
    }

    public uint ID32 { get; }

    public static implicit operator string(SteamID32 steamID32)
    {
        return steamID32.ToString();
    }

    public override string ToString()
    {
        return ID32.ToString();
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