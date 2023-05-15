using SteamTools.ProfileDataFetcher.Models;

namespace SteamTools.ProfileDataFetcher.Services.Interfaces;

public interface ISteamProfileService
{
    Task<SteamProfile> GetProfileAsync(string input);
}