using SteamTools.Domain.Models;

namespace SteamTools.ProfileScanner.Abstractions;

public class AppworkshopData : LocalResult
{
    public AppworkshopData(ISteamIDPair steamIDPair, LocalResultType localResultType) : base(steamIDPair, localResultType) { }
    public required int AppID { get; init; }
}