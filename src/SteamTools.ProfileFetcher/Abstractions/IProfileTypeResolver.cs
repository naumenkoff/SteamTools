using System.Text.RegularExpressions;

namespace SteamTools.ProfileFetcher.Abstractions;

public interface IProfileTypeResolver<T>
{
    Match? GetResolvedMatch(T steamProfileType);
    T ResolveProfileType(string input);
}