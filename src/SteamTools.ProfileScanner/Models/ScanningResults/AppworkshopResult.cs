using SteamTools.Common;
using SteamTools.ProfileScanner.Enums;

namespace SteamTools.ProfileScanner.Models.ScanningResults;

internal sealed class AppworkshopResult(ISteamIDPair steamIdPair) : ResultBase(steamIdPair, ResultType.Appworkshop)
{
    public required int? AppId { get; init; }
}