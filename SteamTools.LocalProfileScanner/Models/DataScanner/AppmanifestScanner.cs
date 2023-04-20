using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using SteamTools.Core.Models;
using SteamTools.Core.Models.Steam;
using SteamTools.LocalProfileScanner.Models.ProfileData;

namespace SteamTools.LocalProfileScanner.Models.DataScanner;

public class AppmanifestScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly ISteamClient _steamClient;

    public AppmanifestScanner(ISteamClient steamClient, Regex pattern)
    {
        _steamClient = steamClient;
        _pattern = pattern;
    }

    public IEnumerable<ISteamID> GetProfiles()
    {
        if (_steamClient.SteamLibraries is null) return Enumerable.Empty<AppmanifestData>();

        var appmanifestFiles = GetAppmanifestFiles();
        var userProfiles = new ConcurrentBag<AppmanifestData>();

        Parallel.ForEach(appmanifestFiles, file =>
        {
            var match = file.GetMatch(_pattern);
            if (!match.Success) return;

            var appmanifestData = CreateAppmanifestData(match);
            userProfiles.Add(appmanifestData);
        });

        return userProfiles;
    }

    private IEnumerable<SingleMatchFile> GetAppmanifestFiles()
    {
        var appmanifestFiles = new List<SingleMatchFile>();

        foreach (var files in from steamLibrary in _steamClient.SteamLibraries
                 select _steamClient.GetSteamappsDirectory(steamLibrary)
                 into steamapps
                 where steamapps is not null
                 select steamapps.GetFiles())
            appmanifestFiles.AddRange(files.Select(file => new SingleMatchFile(file)));

        return appmanifestFiles;
    }

    private static AppmanifestData CreateAppmanifestData(Match match)
    {
        var name = match.Groups[2].Value;
        var steam64 = new SteamID64(long.Parse(match.Groups[3].Value));
        return new AppmanifestData(name, steam64, steam64.ToSteamID32());
    }
}