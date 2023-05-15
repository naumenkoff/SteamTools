using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Models;

public record LoginusersData
    (string Login, string Name, DateTimeOffset Timestamp, SteamID64 Steam64, SteamID32 Steam32) : ISteamIDPair;