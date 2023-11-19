using System.Text.RegularExpressions;
using SteamTools.Domain.Models;
using SteamTools.Domain.Providers;
using SteamTools.Infrastructure.Models;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner.Services;

public class AppworkshopScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly SteamClient _steamClient;

    public AppworkshopScanner(SteamClient steamClient, ITemplateProvider<IScanner> scannerPatternProvider)
    {
        _steamClient = steamClient;
        _pattern = scannerPatternProvider.GetTemplate(this);
    }

    public IEnumerable<LocalResult> GetProfiles()
    {
        return from file in GetAppworkshopFiles() select file.GetMatch(_pattern) into match where match.Success select CreateAppworkshopData(match);
    }

    private IEnumerable<FileMatcher<Match>> GetAppworkshopFiles()
    {
        if (_steamClient.Steam is null) return Enumerable.Empty<FileMatcher<Match>>();

        var appworkshopFiles = new List<FileMatcher<Match>>();

        foreach (var files in from steamLibrary in _steamClient.Steam.GetAnotherInstallations()
                 select steamLibrary.GetSteamappsDirectory()
                 into steamapps
                 where steamapps is not null
                 select _steamClient.GetWorkshopDirectory(steamapps)
                 into workshop
                 where workshop is not null
                 select workshop.GetFiles())
            appworkshopFiles.AddRange(files.Select(file => new FileMatcher<Match>(file)));

        return appworkshopFiles;
    }

    private AppworkshopData CreateAppworkshopData(Match match)
    {
        var steamProfile = new SteamProfile(uint.Parse(match.Groups[2].Value));
        return new AppworkshopData(steamProfile, LocalResultType.Appworkshop)
        {
            AppID = int.Parse(match.Groups[1].Value)
        };
    }
}