using SteamTools.ProfileDataFetcher.Models;

namespace SteamTools.ProfileDataFetcher.Services;

public interface ISteamProfileBuilder
{
    Task<SteamProfile> BuildSteamProfileAsync(string text);
}