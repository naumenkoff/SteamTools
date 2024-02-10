namespace SteamTools.Common;

public interface ISteamIDPair
{
    SteamID32 ID32 { get; }
    SteamID64 ID64 { get; }
}