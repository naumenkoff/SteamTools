using SteamTools.Common;

namespace SteamTools.ProfileFetcher;

public interface IProfileFetcherService
{
    Task<SteamProfile> GetProfileAsync(string input);
}