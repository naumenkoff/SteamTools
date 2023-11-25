using SteamTools.Domain.Models;

namespace SteamTools.ProfileFetcher.Abstractions;

public interface IProfileFetcherService
{
    Task<SteamProfile> GetProfileAsync(string input);
}