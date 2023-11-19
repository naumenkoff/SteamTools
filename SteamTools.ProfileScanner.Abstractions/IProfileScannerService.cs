namespace SteamTools.ProfileScanner.Abstractions;

public interface IProfileScannerService
{
    ValueTask<IEnumerable<LocalResult>> ScanAndGetProfilesAsync();
}