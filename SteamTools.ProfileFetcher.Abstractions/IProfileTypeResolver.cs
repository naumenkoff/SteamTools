using System.Text.RegularExpressions;

namespace SteamTools.ProfileFetcher.Abstractions;

public interface IProfileTypeResolver
{
    Match? GetResolvedMatch(SteamProfileType steamProfileType);
    SteamProfileType ResolveProfileType(string input);
}