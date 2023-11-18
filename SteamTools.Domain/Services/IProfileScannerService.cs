using SteamTools.Domain.Models;

namespace SteamTools.Domain.Services;

public interface IProfileScannerService
{
    ValueTask<IEnumerable<ISteamIDPair>> ScanAndGetProfilesAsync();
}