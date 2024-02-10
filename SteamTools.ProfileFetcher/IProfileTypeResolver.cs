using System.Text.RegularExpressions;

namespace SteamTools.ProfileFetcher;

public interface IProfileTypeResolver
{
    Match? GetResolvedMatch(SteamProfileType steamProfileType);
    SteamProfileType ResolveProfileType(string input);
}