using SteamTools.Common;

namespace SteamTools.ProfileScanner;

public class LocalResult : ISteamIDPair
{
    public LocalResult(ISteamIDPair steamIdPair, LocalResultType localResultType)
    {
        ID32 = steamIdPair.ID32;
        ID64 = steamIdPair.ID64;
        Type = localResultType;
    }

    public LocalResultType Type { get; }
    public SteamID32 ID32 { get; }
    public SteamID64 ID64 { get; }
}