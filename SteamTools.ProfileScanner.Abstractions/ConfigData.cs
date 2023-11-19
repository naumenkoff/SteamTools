using SteamTools.Domain.Models;

namespace SteamTools.ProfileScanner.Abstractions;

public class ConfigData : LocalResult
{
    public ConfigData(ISteamIDPair steamIDPair, LocalResultType localResultType) : base(steamIDPair, localResultType) { }
    public required string Login { get; init; }
}