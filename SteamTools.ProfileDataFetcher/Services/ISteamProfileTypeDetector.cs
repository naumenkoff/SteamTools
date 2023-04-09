using System.Text.RegularExpressions;
using SteamTools.ProfileDataFetcher.Enumerations;

namespace SteamTools.ProfileDataFetcher.Services;

public interface ISteamProfileTypeDetector
{
    Match GetCachedMatchBySteamProfileType(SteamProfileType steamProfileType);
    SteamProfileType DetectSteamProfileType(string input);
}