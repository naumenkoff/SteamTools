using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Models.ProfileData;

public record LoginusersData
    (string Login, string Name, DateTimeOffset Timestamp, SteamID64 Steam64, SteamID32 Steam32) : ISteamID;