using System.Text.RegularExpressions;
using SteamTools.ProfileDataFetcher.Enumerations;

namespace SteamTools.ProfileDataFetcher.Providers;

public interface ISteamProfileRegexProvider
{
    Regex GetRegex(SteamProfileType steamProfileType);
}