using System.Collections.Concurrent;
using System.Diagnostics;
using SteamTools.Domain.Services;
using SteamTools.Infrastructure.Models;
using SteamTools.SignatureSearcher.Abstractions;

namespace SteamTools.SignatureSearcher.Services;

public class ScanningService : IScanningService
{
    private readonly IReadOnlyCollection<string> _extensions;
    private readonly IFileScanner _fileScanner;
    private readonly ParallelOptions _parallelOptions;
    private readonly IScanningResult _scanningResult;
    private readonly SteamClient _steamClient;
    private readonly bool _useExtensions;

    public ScanningService(IFileScanner fileScanner, IScanningResult scanningResult, SteamClient steamClient, ParallelOptions parallelOptions,
        bool useSpecifiedExtensions, IReadOnlyCollection<string> extensions)
    {
        _fileScanner = fileScanner;
        _parallelOptions = parallelOptions;
        _useExtensions = useSpecifiedExtensions;
        _extensions = extensions;
        _scanningResult = scanningResult;
        _steamClient = steamClient;
    }

    public async ValueTask<IScanningResult> StartScanningAsync()
    {
        var start = Stopwatch.GetTimestamp();
        var filesToScan = GetFilesToScan();
        var partitions = Partitioner.Create(filesToScan, EnumerablePartitionerOptions.NoBuffering).GetOrderableDynamicPartitions();
        await Parallel.ForEachAsync(partitions, _parallelOptions,
            async (kvp, token) => { await _fileScanner.ScanFile(kvp.Value, _parallelOptions.CancellationToken); });
        Console.WriteLine(Stopwatch.GetElapsedTime(start).TotalSeconds);
        return _scanningResult;
    }

    private IEnumerable<FileInfo> GetFilesToScan()
    {
        if (_steamClient.Steam is null) return Enumerable.Empty<FileInfo>();

        return _steamClient.Steam.GetAnotherInstallations().SelectMany(steamLibrary =>
        {
            var files = steamLibrary.WorkingDirectory.EnumerateFiles("*.*", SearchOption.AllDirectories);
            return _useExtensions ? files.Where(file => _extensions.Contains(file.Extension)) : files;
        });
    }
}