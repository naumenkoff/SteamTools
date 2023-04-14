namespace SteamTools.LocalProfileScanner.Models;

public interface IScanner
{
    IEnumerable<ISteamID> GetProfiles();
}