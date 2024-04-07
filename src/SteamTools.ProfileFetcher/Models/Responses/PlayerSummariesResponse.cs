using SteamTools.Common;

namespace SteamTools.ProfileFetcher.Models.Responses;

// ReSharper disable ClassNeverInstantiated.Global
public class PlayerSummariesResponse
{
    public List<PlayerSummaries> Players { get; set; } = [];
}