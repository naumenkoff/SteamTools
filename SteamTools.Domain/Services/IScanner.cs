using SteamTools.Domain.Models;

namespace SteamTools.Domain.Services;

public interface IScanner
{
    IEnumerable<ISteamIDPair> GetProfiles();
}