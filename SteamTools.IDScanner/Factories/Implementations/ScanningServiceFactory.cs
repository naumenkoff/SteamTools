using SteamTools.Core.Models;
using SteamTools.Core.Models.Steam;
using SteamTools.IDScanner.Factories.Interfaces;
using SteamTools.IDScanner.Models;
using SteamTools.IDScanner.Services.Implementations;
using SteamTools.IDScanner.Services.Interfaces;

namespace SteamTools.IDScanner.Factories.Implementations;

public class ScanningServiceFactory : IScanningServiceFactory
{
    private readonly IFileScannerFactory _fileScannerFactory;
    private readonly ISteamClient _steamClient;
    private readonly ISteamIDValidatorFactory _steamIDValidatorFactory;

    public ScanningServiceFactory(ISteamClient steamClient, IFileScannerFactory fileScannerFactory,
        ISteamIDValidatorFactory steamIDValidatorFactory)
    {
        _steamClient = steamClient;
        _fileScannerFactory = fileScannerFactory;
        _steamIDValidatorFactory = steamIDValidatorFactory;
    }

    public IScanningService Create(SteamID64 steamID64, bool limitMaximumFileSize,
        long maximumFileSizeInBytes, bool useSpecifiedExtensions, int processorCount,
        CancellationToken cancellationToken, params string[] extensions)
    {
        var parallelOptions = new ParallelOptions
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = processorCount
        };

        var scanningResult = ScanningResult.Empty;
        var fileScanValidator = _steamIDValidatorFactory.Create(steamID64);
        var fileScanner = _fileScannerFactory.Create(scanningResult, fileScanValidator, limitMaximumFileSize,
            maximumFileSizeInBytes);
        return new ScanningService(fileScanner, scanningResult, _steamClient, parallelOptions, useSpecifiedExtensions,
            extensions);
    }
}