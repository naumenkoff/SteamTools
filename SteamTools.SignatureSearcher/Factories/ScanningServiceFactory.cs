using SteamTools.Domain.Models;
using SteamTools.Infrastructure.Models;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Services;

namespace SteamTools.SignatureSearcher.Factories;

public class ScanningServiceFactory : IScanningServiceFactory
{
    private readonly ScanningOptions _scanningOptions;
    private readonly SteamClient _steamClient;

    public ScanningServiceFactory(SteamClient steamClient, ScanningOptions scanningOptions)
    {
        _scanningOptions = scanningOptions;
        _steamClient = steamClient;
    }

    public IScanningService Create(SteamProfile steamProfile, CancellationToken cancellationToken)
    {
        var parallelOptions = new ParallelOptions
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = _scanningOptions.Processors
        };

        var scanningResult = new ScanningResult();
        var fileScanValidator = new FileValidator(steamProfile);
        var fileScanner = new FileScanner(scanningResult, fileScanValidator, _scanningOptions.LimitScanningFileSize,
            _scanningOptions.GetFormattedMaximumFileSize());
        return new ScanningService(fileScanner, scanningResult, _steamClient, parallelOptions, _scanningOptions.ScanFilesOnlyWithSpecifiedExtensions,
            _scanningOptions.Extensions);
    }
}