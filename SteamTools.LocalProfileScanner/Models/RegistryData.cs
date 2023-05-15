using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Models;

public record RegistryData(SteamID64 Steam64, SteamID32 Steam32) : ISteamIDPair;