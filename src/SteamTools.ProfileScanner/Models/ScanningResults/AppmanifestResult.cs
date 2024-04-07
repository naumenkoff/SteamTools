using SteamTools.Common;
using SteamTools.ProfileScanner.Enums;

namespace SteamTools.ProfileScanner.Models.ScanningResults;

internal sealed class AppmanifestResult(ISteamIDPair steamIdPair) : ResultBase(steamIdPair, ResultType.Appmanifest)
{
    public required string? Name { get; init; }
}