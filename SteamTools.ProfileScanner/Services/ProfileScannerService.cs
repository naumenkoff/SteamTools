using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner.Services;

public class ProfileScannerService : IProfileScannerService
{
    private readonly IEnumerable<IScanner> _scanners;

    public ProfileScannerService(IEnumerable<IScanner> scanners)
    {
        _scanners = scanners;
    }

    public ValueTask<IEnumerable<LocalResult>> ScanAndGetProfilesAsync()
    {
        var profiles = _scanners.SelectMany(x => x.GetProfiles());
        return new ValueTask<IEnumerable<LocalResult>>(profiles);
    }
}