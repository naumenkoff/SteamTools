using System.Text.RegularExpressions;
using SteamTools.Core.Models;
using SteamTools.Core.Models.Steam;
using SteamTools.LocalProfileScanner.Models;
using SteamTools.LocalProfileScanner.Services.Interfaces;
using SteamTools.LocalProfileScanner.Utilities;

namespace SteamTools.LocalProfileScanner.Services.Implementations;

public class AppmanifestScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly ISteamClient _steamClient;

    public AppmanifestScanner(ISteamClient steamClient, Regex pattern)
    {
        _steamClient = steamClient;
        _pattern = pattern;
    }

    public IEnumerable<ISteamIDPair> GetProfiles()
    {
        if (_steamClient.SteamLibraries is null) return Enumerable.Empty<AppmanifestData>();

        var appmanifestFiles = GetAppmanifestFiles();
        var userProfiles = new List<AppmanifestData>();
        Parallel.ForEach(appmanifestFiles, file =>
        {
            var match = file.GetMatch(_pattern);
            if (!match.Success) return;

            var appmanifestData = CreateAppmanifestData(match);
            userProfiles.Add(appmanifestData);
        });

        return userProfiles;
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
        var steam64 = new SteamID64(long.Parse(match.Groups[3].Value));
        return new AppmanifestData(name, steam64, steam64.ToSteamID32());
    }
}