namespace SteamTools.ProfileScanner.Abstractions;

public interface IProfileScannerService
{
    ValueTask<IEnumerable<LocalProfile>> GetProfiles();
}