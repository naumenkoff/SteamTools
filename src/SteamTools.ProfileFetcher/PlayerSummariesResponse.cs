using SteamTools.Common;

namespace SteamTools.ProfileFetcher;

// ReSharper disable ClassNeverInstantiated.Global
public class PlayerSummariesResponse
{
    public List<PlayerSummaries> Players { get; set; } = [];
}