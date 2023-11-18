using SteamTools.Domain.Models;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services;

public class ProfileScannerService : IProfileScannerService
{
    private readonly IEnumerable<IScanner> _scanners;

    public ProfileScannerService(IEnumerable<IScanner> scanners)
    {
        _scanners = scanners;
    }

    public ValueTask<IEnumerable<ISteamIDPair>> ScanAndGetProfilesAsync()
    {
        var profiles = _scanners.SelectMany(x => x.GetProfiles());
        return new ValueTask<IEnumerable<ISteamIDPair>>(profiles);
    }
}