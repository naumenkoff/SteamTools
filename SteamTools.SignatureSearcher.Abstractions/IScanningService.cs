using SteamTools.Domain.Models;

namespace SteamTools.SignatureSearcher.Abstractions;

public interface IScanningService
{
    Task<IScanningResult?> StartScanningAsync(ISteamIDPair steamId, CancellationToken cancellationToken);
}