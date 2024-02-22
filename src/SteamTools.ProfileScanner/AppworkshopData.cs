using SteamTools.Common;

namespace SteamTools.ProfileScanner;

internal sealed class AppworkshopData(ISteamIDPair steamIdPair, LocalResultType localResultType) : LocalResult(steamIdPair, localResultType)
{
    public required int? AppId { get; init; }
}