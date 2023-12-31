using SteamTools.Domain.Models;

namespace SteamTools.ProfileScanner.Abstractions;

public class LoginusersData : LocalResult
{
    public LoginusersData(ISteamIDPair steamIdPair, LocalResultType localResultType) : base(steamIdPair, localResultType) { }
    public required string? Login { get; init; }
    public required string? Name { get; init; }
    public required DateTimeOffset? Timestamp { get; init; }
}