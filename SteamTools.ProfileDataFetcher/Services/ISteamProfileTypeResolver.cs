using System.Text.RegularExpressions;
using SteamTools.ProfileDataFetcher.Enumerations;

namespace SteamTools.ProfileDataFetcher.Services;

public interface ISteamProfileTypeResolver
{
    Match GetCachedMatchBySteamProfileType(SteamProfileType steamProfileType);
    SteamProfileType ResolveSteamProfileType(string input);
}