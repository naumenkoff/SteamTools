using SteamTools.Domain.Models;

namespace SteamTools.ProfileScanner.Abstractions;

public class AppmanifestData : LocalResult
{
    public AppmanifestData(ISteamIDPair steamIDPair, LocalResultType localResultType) : base(steamIDPair, localResultType) { }
    public required string Name { get; init; }
}