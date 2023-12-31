using SteamTools.Domain.Models;

namespace SteamTools.ProfileScanner.Abstractions;

public class AppworkshopData : LocalResult
{
    public AppworkshopData(ISteamIDPair steamIdPair, LocalResultType localResultType) : base(steamIdPair, localResultType) { }
    public required int? AppId { get; init; }
}