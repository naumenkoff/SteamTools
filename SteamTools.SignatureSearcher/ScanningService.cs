using System.Collections.Concurrent;
using SProject.FileSystem;
using SteamTools.Common;

namespace SteamTools.SignatureSearcher;

internal class ScanningService : IScanningService
{
    private readonly IFileScanner _fileScanner;
    private readonly ScanningOptions _scanningOptions;
    private readonly SteamClient _steamClient;

    public ScanningService(IFileScanner fileScanner, SteamClient steamClient, ScanningOptions scanningOptions)
    {
        _fileScanner = fileScanner;
        _steamClient = steamClient;
        _scanningOptions = scanningOptions;
    }

    public async Task<IScanningResult?> StartScanningAsync(ISteamIDPair steamId, CancellationToken cancellationToken)
    {
        if (_steamClient.Steam is null) return null;

        var parallelOptions = new ParallelOptions
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = _scanningOptions.Processors
        };

        var filesToScan = GetFilesToScan();
        var partitions = Partitioner.Create(filesToScan, EnumerablePartitionerOptions.NoBuffering).GetOrderableDynamicPartitions();

        _fileScanner.Initialize(steamId);

        await Parallel.ForEachAsync(partitions, parallelOptions,
            async (kvp, _) => { await _fileScanner.ScanFile(kvp.Value, cancellationToken).ConfigureAwait(false); }).ConfigureAwait(false);

        return _fileScanner.GetResult();
    }

    private IEnumerable<FileInfo> GetFilesToScan()
    {
        var files = _steamClient.Steam!.GetSteamLibraries().SelectMany(steamLibrary => steamLibrary.WorkingDirectory.EnumerateAllFiles());
        return _scanningOptions.ScanFilesOnlyWithSpecifiedExtensions
            ? files.Where(file => _scanningOptions.Extensions.Contains(file.Extension))
            : files;
    }
}