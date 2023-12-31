using SteamTools.Domain.Models;

namespace SteamTools.ProfileScanner.Abstractions;

public class AppmanifestData : LocalResult
{
    public AppmanifestData(ISteamIDPair steamIdPair, LocalResultType localResultType) : base(steamIdPair, localResultType) { }
    public required string? Name { get; init; }
}