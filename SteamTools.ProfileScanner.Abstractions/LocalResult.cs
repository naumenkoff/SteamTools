using SteamTools.Domain.Models;

namespace SteamTools.ProfileScanner.Abstractions;

public class LocalResult : ISteamIDPair
{
    public LocalResult(ISteamIDPair steamIDPair, LocalResultType localResultType)
    {
        ID32 = steamIDPair.ID32;
        ID64 = steamIDPair.ID64;
        Type = localResultType;
    }

    public LocalResultType Type { get; }
    public SteamID32 ID32 { get; }
    public SteamID64 ID64 { get; }
}