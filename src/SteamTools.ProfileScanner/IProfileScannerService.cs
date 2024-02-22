namespace SteamTools.ProfileScanner;

public interface IProfileScannerService
{
    ValueTask<IEnumerable<LocalProfile>> GetProfiles();
}