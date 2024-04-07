using SteamTools.Common;
using SteamTools.ProfileScanner.Enums;

namespace SteamTools.ProfileScanner.Models.ScanningResults;

internal sealed class LoginusersResult(ISteamIDPair steamIdPair) : ResultBase(steamIdPair, ResultType.Loginusers)
{
    public required string? Login { get; init; }
    public required string? Name { get; init; }
    public required DateTimeOffset? Timestamp { get; init; }
}