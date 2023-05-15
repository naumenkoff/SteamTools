using SteamTools.Core.Models;

namespace SteamTools.LocalProfileScanner.Services.Interfaces;

public interface IScanner
{
    IEnumerable<ISteamIDPair> GetProfiles();
}