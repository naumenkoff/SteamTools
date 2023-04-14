using SteamTools.Core.Models;

namespace SteamTools.IDScanner.Services;

public interface IScanningService
{
    int TotalFileCount { get; }
    int ScannedFileCount { get; }

    Task<List<string>> StartScanning(SteamID64 steamID64, bool maximumFileSizeLimit, long maximumFileSize,
        bool hasExtensions, CancellationToken cancellationToken, params string[] extensions);
}