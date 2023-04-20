using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Models.ProfileData;

public record RegistryData(SteamID64 Steam64, SteamID32 Steam32) : ISteamID;