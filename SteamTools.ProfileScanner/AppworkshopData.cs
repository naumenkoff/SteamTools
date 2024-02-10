using SteamTools.Common;

namespace SteamTools.ProfileScanner;

public class AppworkshopData(ISteamIDPair steamIdPair, LocalResultType localResultType) : LocalResult(steamIdPair, localResultType)
{
    public required int? AppId { get; init; }
}