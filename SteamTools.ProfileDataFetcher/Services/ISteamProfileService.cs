using SteamTools.ProfileDataFetcher.Models;

namespace SteamTools.ProfileDataFetcher.Services;

public interface ISteamProfileService
{
    Task<SteamProfile> GetProfileAsync(string input);
}