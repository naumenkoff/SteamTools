using SteamTools.Common;

namespace SteamTools.ProfileScanner;

internal sealed class LoginusersData(ISteamIDPair steamIdPair, LocalResultType localResultType) : LocalResult(steamIdPair, localResultType)
{
    public required string? Login { get; init; }
    public required string? Name { get; init; }
    public required DateTimeOffset? Timestamp { get; init; }
}