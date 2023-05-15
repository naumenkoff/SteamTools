using System.Text.RegularExpressions;
using SteamTools.ProfileDataFetcher.Enumerations;

namespace SteamTools.ProfileDataFetcher.Services.Interfaces;

public interface ISteamProfileTypeDetector
{
    Match GetCachedMatchBySteamProfileType(SteamProfileType steamProfileType);
    SteamProfileType DetectSteamProfileType(string input);
}