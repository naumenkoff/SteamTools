using System.Text.RegularExpressions;
using SteamTools.Domain.Models;
using SteamTools.Domain.Models.LScanning;
using SteamTools.Domain.Providers;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services.LScanning;

public class AppworkshopScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly ISteamClient _steamClient;

    public AppworkshopScanner(ISteamClient steamClient, ITemplateProvider<IScanner> scannerPatternProvider)
    {
        _steamClient = steamClient;
        _pattern = scannerPatternProvider.GetTemplate(this);
    }

    public IEnumerable<ISteamIDPair> GetProfiles()
    {
        return from file in GetAppworkshopFiles() select file.GetMatch(_pattern) into match where match.Success select CreateAppworkshopData(match);
    }

    private IEnumerable<FileMatcher<Match>> GetAppworkshopFiles()
    {
        var appworkshopFiles = new List<FileMatcher<Match>>();

        foreach (var files in from steamLibrary in _steamClient.SteamLibraries
                 select _steamClient.GetSteamappsDirectory(steamLibrary)
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
        var appID = int.Parse(match.Groups[1].Value);
        var steamProfile = new SteamProfile(uint.Parse(match.Groups[2].Value));
        return new AppworkshopData(appID, steamProfile);
    }
}