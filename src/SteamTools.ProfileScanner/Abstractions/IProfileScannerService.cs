using SteamTools.ProfileScanner.Models;

namespace SteamTools.ProfileScanner.Abstractions;

public interface IProfileScannerService
{
    ValueTask<IEnumerable<ResultProfile>> GetProfiles();
}