using System.Text.RegularExpressions;

namespace SteamTools.ProfileFetcher;

public interface IProfileTypeResolver<T>
{
    Match? GetResolvedMatch(T steamProfileType);
    T ResolveProfileType(string input);
}