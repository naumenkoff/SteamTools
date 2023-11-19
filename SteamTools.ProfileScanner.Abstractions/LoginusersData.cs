using SteamTools.Domain.Models;

namespace SteamTools.ProfileScanner.Abstractions;

public class LoginusersData : LocalResult
{
    public LoginusersData(ISteamIDPair steamIDPair, LocalResultType localResultType) : base(steamIDPair, localResultType) { }
    public required string Login { get; init; }
    public required string Name { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
}