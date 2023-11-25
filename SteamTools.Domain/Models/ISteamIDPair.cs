namespace SteamTools.Domain.Models;

public interface ISteamIDPair
{
    SteamID32 ID32 { get; }
    SteamID64 ID64 { get; }
}