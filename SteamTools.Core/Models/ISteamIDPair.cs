namespace SteamTools.Core.Models;

public interface ISteamIDPair
{
    SteamID32 Steam32 { get; }
    SteamID64 Steam64 { get; }
}