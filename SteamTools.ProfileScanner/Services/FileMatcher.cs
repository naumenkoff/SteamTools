using System.Text.RegularExpressions;
using SProject.FileSystem;

namespace SteamTools.ProfileScanner.Services;

public class FileMatcher<T>
{
    private readonly FileInfo _file;

    public FileMatcher(FileInfo file)
    {
        _file = file;
    }

    public T GetMatch(Regex pattern)
    {
        var content = _file.ReadAllText();
        object returnValue;

        if (typeof(T) == typeof(Match))
            returnValue = pattern.Match(content);
        else if (typeof(T) == typeof(IEnumerable<Match>))
            returnValue = pattern.Matches(content);
        else
            throw new InvalidOperationException($"Unsupported return type: {typeof(T)}");

        return (T) returnValue;
    }
}