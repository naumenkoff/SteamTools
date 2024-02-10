using SteamTools.Common;

namespace SteamTools.ProfileScanner;

public class AppmanifestData(ISteamIDPair steamIdPair, LocalResultType localResultType) : LocalResult(steamIdPair, localResultType)
{
    public required string? Name { get; init; }
}