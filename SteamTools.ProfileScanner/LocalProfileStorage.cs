using System.Diagnostics;
using SteamTools.Domain.Extensions;
using SteamTools.Domain.Models;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner;

public class LocalProfileStorage : ILocalProfileStorage
{
    private readonly List<LocalProfile> _accounts;
    private readonly IProfileScannerService _profileScannerService;

    public LocalProfileStorage(IProfileScannerService profileScannerService)
    {
        _accounts = new List<LocalProfile>();
        _profileScannerService = profileScannerService;
    }

    public async Task InitializeAsync()
    {
        var start = Stopwatch.GetTimestamp();
        var profiles = await _profileScannerService.ScanAndGetProfilesAsync();
        foreach (var detectedProfile in profiles)
        {
            var profile = GetAccount(detectedProfile);
            profile.Attach(detectedProfile);
        }

        Console.WriteLine($"{GetType().FullName} | Initialization time => {Stopwatch.GetElapsedTime(start).TotalSeconds} sec.");
    }

    public IEnumerable<LocalProfile> Accounts => _accounts;

    public LocalProfile GetAccount(ISteamIDPair steamData)
    {
        var account = Accounts.FirstOrDefault(x => x.IsMatch(steamData));
        if (account is not null) return account;

        account = new LocalProfile(steamData);
        _accounts.Add(account);
        return account;
    }
}