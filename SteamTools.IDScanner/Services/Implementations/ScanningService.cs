using System.Collections.Concurrent;
using SteamTools.Core.Models.Steam;
using SteamTools.IDScanner.Models;
using SteamTools.IDScanner.Services.Interfaces;

namespace SteamTools.IDScanner.Services.Implementations;

public class ScanningService : IScanningService
{
    private readonly string[] _extensions;
    private readonly IFileScanner _fileScanner;
    private readonly ParallelOptions _parallelOptions;
    private readonly IScanningResult _scanningResult;
    private readonly ISteamClient _steamClient;
    private readonly bool _useExtensions;

    public ScanningService(IFileScanner fileScanner, IScanningResult scanningResult, ISteamClient steamClient,
        ParallelOptions parallelOptions, bool useSpecifiedExtensions, string[] extensions)
    {
        _fileScanner = fileScanner;
        _parallelOptions = parallelOptions;
        _useExtensions = useSpecifiedExtensions;
        _extensions = extensions;
        _scanningResult = scanningResult;
        _steamClient = steamClient;
    }

    public async Task<IScanningResult> StartScanningAsync()
    {
        await ScanAsync();
        return _scanningResult;
    }

    private async Task ScanAsync()
    {
        var files = await GetFilesToScanAsync();
        await ScanFilesAsync(files);
    }

    private async Task<IEnumerable<FileInfo>> GetFilesToScanAsync()
    {
        var filesToScan = new ConcurrentBag<FileInfo>();

        await Parallel.ForEachAsync(_steamClient.SteamLibraries, _parallelOptions, (steamLibrary, _) =>
        {
            var files = steamLibrary.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (var file in _useExtensions
                         ? files.Where(file => _extensions.Contains(file.Extension))
                         : files)
                filesToScan.Add(file);

            return ValueTask.CompletedTask;
        });

        return filesToScan;
    }

    private async Task ScanFilesAsync(IEnumerable<FileInfo> files)
    {
        await Task.Run(() =>
        {
            var partitions = Partitioner.Create(files, EnumerablePartitionerOptions.NoBuffering);
            Parallel.ForEach(partitions, _parallelOptions,
                partition => { _fileScanner.ScanFile(partition, _parallelOptions.CancellationToken); });
        });
    }
}