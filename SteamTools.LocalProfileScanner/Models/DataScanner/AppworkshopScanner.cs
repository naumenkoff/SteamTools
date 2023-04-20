using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using SteamTools.Core.Models;
using SteamTools.Core.Models.Steam;
using SteamTools.LocalProfileScanner.Models.ProfileData;

namespace SteamTools.LocalProfileScanner.Models.DataScanner;

public class AppworkshopScanner : IScanner
{
    private readonly Regex _pattern;
    private readonly ISteamClient _steamClient;

    public AppworkshopScanner(ISteamClient steamClient, Regex pattern)
    {
        _steamClient = steamClient;
        _pattern = pattern;
    }

    public IEnumerable<ISteamID> GetProfiles()
    {
        if (_steamClient.SteamLibraries is null) return Enumerable.Empty<AppworkshopData>();

        var appworkshopFiles = GetAppworkshopFiles();
        var userProfiles = new ConcurrentBag<AppworkshopData>();

        Parallel.ForEach(appworkshopFiles, file =>
        {
            var match = file.GetMatch(_pattern);
            if (!match.Success) return;

            var appworkshopData = CreateAppworkshopData(match);
            userProfiles.Add(appworkshopData);
        });

        return userProfiles;
    }

    private IEnumerable<SingleMatchFile> GetAppworkshopFiles()
    {
        var appworkshopFiles = new List<SingleMatchFile>();

        foreach (var files in from steamLibrary in _steamClient.SteamLibraries
                 select _steamClient.GetSteamappsDirectory(steamLibrary)
                 into steamapps
                 where steamapps is not null
                 select _steamClient.GetWorkshopDirectory(steamapps)
                 into workshop
                 where workshop is not null
                 select workshop.GetFiles()) appworkshopFiles.AddRange(files.Select(file => new SingleMatchFile(file)));

        return appworkshopFiles;
    }

    private static AppworkshopData CreateAppworkshopData(Match match)
    {
        var appID = int.Parse(match.Groups[1].Value);
        var steam32 = new SteamID32(uint.Parse(match.Groups[2].Value));
        return new AppworkshopData(appID, steam32.ToSteamID64(), steam32);
    }
}