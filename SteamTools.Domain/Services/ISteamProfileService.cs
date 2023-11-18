using SteamTools.Domain.Models;

namespace SteamTools.Domain.Services;

public interface ISteamProfileService
{
    Task<SteamProfile> GetProfileAsync(string input);
}