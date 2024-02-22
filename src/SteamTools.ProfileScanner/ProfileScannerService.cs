using System.Diagnostics;
using SProject.Steam;
using SteamTools.Common;

namespace SteamTools.ProfileScanner;

internal sealed class ProfileScannerService(IEnumerable<IScanner> scanners) : IProfileScannerService
{
    public ValueTask<IEnumerable<LocalProfile>> GetProfiles()
    {
        var accounts = new List<LocalProfile>();
        var start = Stopwatch.GetTimestamp();

        foreach (var localResult in scanners.AsParallel().SelectMany(x => x.GetProfiles()))
        {
            if (!SteamIDValidator.IsSteamID64(localResult.ID64.AsLong)) continue;

            var account = accounts.FirstOrDefault(x => x.IsMatch(localResult));
            if (account is null)
            {
                account = new LocalProfile(localResult);
                accounts.Add(account);
                continue;
            }

            account.Attach(localResult);
        }

        Console.WriteLine($"{GetType().FullName} | Initialization time => {Stopwatch.GetElapsedTime(start).TotalSeconds} sec.");
        return new ValueTask<IEnumerable<LocalProfile>>(accounts);
    }
}