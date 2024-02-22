using SteamTools.Common;

namespace SteamTools.SignatureSearcher;

public interface IScanningService
{
    Task<IScanningResult?> StartScanningAsync(ISteamIDPair steamId, CancellationToken cancellationToken);
}