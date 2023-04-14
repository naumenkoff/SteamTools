using System.Text.RegularExpressions;
using SteamTools.Core.Utilities;

namespace SteamTools.LocalProfileScanner.Models;

public class SingleMatchFile
{
    private readonly FileInfo _file;

    public SingleMatchFile(FileInfo file)
    {
        _file = file;
    }

    private string GetContent()
    {
        return FileSystemHelper.ReadAllText(_file);
    }

    public Match GetMatch(Regex pattern)
    {
        var content = GetContent();
        return pattern.Match(content);
    }
}