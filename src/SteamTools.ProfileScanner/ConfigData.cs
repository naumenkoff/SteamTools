using SteamTools.Common;

namespace SteamTools.ProfileScanner;

internal sealed class ConfigData(ISteamIDPair steamIdPair, LocalResultType localResultType) : LocalResult(steamIdPair, localResultType)
{
    public required string Login { get; init; }
}