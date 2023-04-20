using SteamTools.Core.Models;
using SteamTools.IDScanner.Models;

namespace SteamTools.IDScanner.Services;

public interface IScanningService
{
    Task<IScanningResult> StartScanning(SteamID64 steamID64, bool limitMaximumFileSize,
        long maximumFileSizeInBytes, bool useSpecifiedExtensions, CancellationToken cancellationToken,
        params string[] extensions);
}