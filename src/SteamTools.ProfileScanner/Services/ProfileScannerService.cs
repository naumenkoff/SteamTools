using System.Diagnostics;
using SProject.Steam;
using SteamTools.Common;
using SteamTools.ProfileScanner.Abstractions;
using SteamTools.ProfileScanner.Models;

namespace SteamTools.ProfileScanner.Services;

internal sealed class ProfileScannerService(IEnumerable<IScanner> scanners) : IProfileScannerService
{
    public ValueTask<IEnumerable<ResultProfile>> GetProfiles()
    {
        var accounts = new List<ResultProfile>();
        var start = Stopwatch.GetTimestamp();
        foreach (var localResult in scanners.AsParallel().SelectMany(x => x.EnumerateProfiles()))
        {
            if (!SteamIDValidator.IsSteamID64(localResult.ID64.AsLong)) continue;
            var account = accounts.FirstOrDefault(x => x.IsMatch(localResult));
            if (account is null)
            {
                account = new ResultProfile(localResult);
                accounts.Add(account);
                continue;
            }

            account.Attach(localResult);
        }

        Console.WriteLine($"{GetType().FullName} | Initialization time => {Stopwatch.GetElapsedTime(start).TotalSeconds} sec.");
        return new ValueTask<IEnumerable<ResultProfile>>(accounts);
    }
}