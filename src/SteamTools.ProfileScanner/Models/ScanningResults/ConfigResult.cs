using SteamTools.Common;
using SteamTools.ProfileScanner.Enums;

namespace SteamTools.ProfileScanner.Models.ScanningResults;

internal sealed class ConfigResult(ISteamIDPair steamIdPair) : ResultBase(steamIdPair, ResultType.Config)
{
    public required string Login { get; init; }
}