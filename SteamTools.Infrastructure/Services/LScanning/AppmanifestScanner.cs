using System.Text.RegularExpressions;
using SteamTools.Domain.Models;
using SteamTools.Domain.Models.LScanning;
using SteamTools.Domain.Providers;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services.LScanning;

public class AppmanifestScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly Steam _steamClient;

    public AppmanifestScanner(Steam steam, ITemplateProvider<IScanner> scannerPatternProvider)
    {
        _steamClient = steamClient;
        _pattern = scannerPatternProvider.GetTemplate(this);
    }

    public IEnumerable<ISteamIDPair> GetProfiles()
    {
        return from file in GetAppmanifestFiles() select file.GetMatch(_pattern) into match where match.Success select CreateAppmanifestData(match);
    }

    private IEnumerable<FileMatcher<Match>> GetAppmanifestFiles()
    {
        var appmanifestFiles = new List<FileMatcher<Match>>();

        foreach (var files in from steamLibrary in _steamClient.SteamLibraries
                 select _steamClient.GetSteamappsDirectory(steamLibrary)
                 into steamapps
                 where steamapps is not null
                 select steamapps.GetFiles())
            appmanifestFiles.AddRange(files.Select(file => new FileMatcher<Match>(file)));

        return appmanifestFiles;
    }

    private static AppmanifestData CreateAppmanifestData(Match match)
    {
        var name = match.Groups[2].Value;
        var steamProfile = new SteamProfile(long.Parse(match.Groups[3].Value));
        return new AppmanifestData(name, steamProfile);
    }
}