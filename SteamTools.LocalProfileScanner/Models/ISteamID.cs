using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Models;

public interface ISteamID
{
    SteamID32 Steam32 { get; }
    SteamID64 Steam64 { get; }
}