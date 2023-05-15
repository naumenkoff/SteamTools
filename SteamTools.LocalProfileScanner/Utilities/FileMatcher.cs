using System.Text.RegularExpressions;
using SteamTools.Core.Utilities;

namespace SteamTools.LocalProfileScanner.Utilities;

public class FileMatcher<T>
{
    private readonly FileInfo _file;

    public FileMatcher(FileInfo file)
    {
        _file = file;
    }

    private string GetContent()
    {
        return FileSystemHelper.ReadAllText(_file);
    }

    public T GetMatch(Regex pattern)
    {
        var content = GetContent();
        object returnValue;

        if (typeof(T) == typeof(Match))
            returnValue = pattern.Match(content);
        else if (typeof(T) == typeof(IEnumerable<Match>))
            returnValue = pattern.Matches(content);
        else throw new InvalidOperationException($"Unsupported return type: {typeof(T)}");

        return (T)returnValue;
    }
}