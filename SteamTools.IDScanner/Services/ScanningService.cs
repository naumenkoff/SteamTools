using SteamTools.Core.Models;
using SteamTools.Core.Models.Steam;
using SteamTools.IDScanner.Models;

namespace SteamTools.IDScanner.Services;

public class ScanningService : IScanningService
{
    private readonly ScanningResult _scanningResult;
    private readonly ISteamClient _steamClient;
    private IFileScanner _fileScanner;
    private ParallelOptions _parallelOptions;

    public ScanningService(ISteamClient steamClient, ScanningResult scanningResult)
    {
        _steamClient = steamClient;
        _scanningResult = scanningResult;
    }

    public async Task<IScanningResult> StartScanningAsync(SteamID64 steamID64, bool limitMaximumFileSize,
        long maximumFileSizeInBytes, bool useSpecifiedExtensions, CancellationToken cancellationToken,
        params string[] extensions)
    {
        var availableCores = Environment.ProcessorCount - 1;
        var cores = useSpecifiedExtensions ? availableCores / 2 : availableCores;

        _parallelOptions = new ParallelOptions
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = Math.Max(cores, 1)
        };
        _fileScanner = new StreamFileScanner(_scanningResult, steamID64, limitMaximumFileSize, maximumFileSizeInBytes);
        await ScanAsync(useSpecifiedExtensions, extensions);
        return _scanningResult;
    }

    private async Task ScanAsync(bool useSpecifiedExtensions, string[] extensions)
    {
        await Parallel.ForEachAsync(_steamClient.SteamLibraries, _parallelOptions, (steamLibrary, _) =>
        {
            if (useSpecifiedExtensions) ScanFilesWithSpecifiedExtensions(extensions, steamLibrary);
            else ScanAllFiles(steamLibrary);

            return ValueTask.CompletedTask;
        });
    }

    private void ScanAllFiles(DirectoryInfo directory)
    {
        var files = directory.GetFiles("*.*", SearchOption.AllDirectories);
        ScanFiles(files);
    }

    private void ScanFilesWithSpecifiedExtensions(IEnumerable<string> extensions, DirectoryInfo directory)
    {
        Parallel.ForEach(extensions, _parallelOptions, extension =>
        {
            var files = directory.GetFiles(extension, SearchOption.AllDirectories);
            ScanFiles(files);
        });
    }

    private void ScanFiles(IEnumerable<FileInfo> files)
    {
        Parallel.ForEach(files, _parallelOptions,
            file => { _fileScanner.ScanFile(file, _parallelOptions.CancellationToken); });
    }
}