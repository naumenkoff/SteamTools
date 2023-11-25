using SteamTools.Domain.Models;

namespace SteamTools.ProfileScanner.Abstractions;

public class ConfigData : LocalResult
{
    public ConfigData(ISteamIDPair steamIdPair, LocalResultType localResultType) : base(steamIdPair, localResultType) { }
    public required string Login { get; init; }
}