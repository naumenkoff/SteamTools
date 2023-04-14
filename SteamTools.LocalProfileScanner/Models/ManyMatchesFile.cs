using System.Text.RegularExpressions;
using SteamTools.Core.Utilities;

namespace SteamTools.LocalProfileScanner.Models;

public class ManyMatchesFile
{
    private readonly FileInfo _file;

    public ManyMatchesFile(FileInfo file)
    {
        _file = file;
    }

    private string GetContent()
    {
        return FileSystemHelper.ReadAllText(_file);
    }

    public IEnumerable<Match> GetMatches(Regex pattern)
    {
        var content = GetContent();
        return pattern.Matches(content);
    }
}