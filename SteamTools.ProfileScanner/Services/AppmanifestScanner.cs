using System.Text.RegularExpressions;
using SteamTools.Domain.Models;
using SteamTools.Domain.Providers;
using SteamTools.Infrastructure.Models;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner.Services;

public class AppmanifestScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly SteamClient _steamClient;

    public AppmanifestScanner(SteamClient steamClient, ITemplateProvider<IScanner> scannerPatternProvider)
    {
        _steamClient = steamClient;
        _pattern = scannerPatternProvider.GetTemplate(this);
    }

    public IEnumerable<LocalResult> GetProfiles()
    {
        return from file in GetAppmanifestFiles() select file.GetMatch(_pattern) into match where match.Success select CreateAppmanifestData(match);
    }

    private IEnumerable<FileMatcher<Match>> GetAppmanifestFiles()
    {
        if (_steamClient.Steam is null) return Enumerable.Empty<FileMatcher<Match>>();

        var appmanifestFiles = new List<FileMatcher<Match>>();

        foreach (var files in from steamLibrary in _steamClient.Steam.GetAnotherInstallations()
                 select steamLibrary.GetSteamappsDirectory()
                 into steamapps
                 where steamapps is not null
                 select steamapps.GetFiles())
            appmanifestFiles.AddRange(files.Select(file => new FileMatcher<Match>(file)));

        return appmanifestFiles;
    }

    private static AppmanifestData CreateAppmanifestData(Match match)
    {
        var steamProfile = new SteamProfile(long.Parse(match.Groups[3].Value));
        return new AppmanifestData(steamProfile, LocalResultType.Appmanifest)
        {
            Name = match.Groups[2].Value
        };
    }
}