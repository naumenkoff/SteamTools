using System.Text.RegularExpressions;
using SteamTools.Domain.Enumerations;

namespace SteamTools.Domain.Services;

public interface ISteamProfileTypeDetector
{
    Match? GetCachedMatchBySteamProfileType(SteamProfileType steamProfileType);
    SteamProfileType ResolveSteamProfileType(string input);
}