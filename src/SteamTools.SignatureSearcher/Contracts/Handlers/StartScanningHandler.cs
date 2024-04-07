using System.Collections.Concurrent;
using SProject.CQRS;
using SteamTools.Common;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Contracts.Requests;
using SteamTools.SignatureSearcher.Contracts.Responses;

namespace SteamTools.SignatureSearcher.Contracts.Handlers;

internal sealed class StartScanningHandler(
    IFactory<IFileProvider> fileProviderFactory,
    IScanningResultBuilder scanningResultBuilder,
    IFactory<ISteamIDPair, IFileScanner> fileScannerFactory,
    ScanningOptions scanningOptions) : IRequestHandler<StartScanningRequest, ScanningResult>
{
    public async Task<ScanningResult> ExecuteAsync(StartScanningRequest request)
    {
        var fileProvider = fileProviderFactory.Create();
        var fileScanner = fileScannerFactory.Create(request.SteamId);
        var parallelOptions = CreateParallelOptions(request.ScanningCancellation);
        var filesToScan = fileProvider.EnumerateSuitableFiles();
        var partitions = Partitioner.Create(filesToScan, EnumerablePartitionerOptions.NoBuffering).GetOrderableDynamicPartitions();
        await Parallel.ForEachAsync(partitions, parallelOptions, (pair, token) =>
        {
            var result = fileScanner.ScanFile(pair.Value, token);
            scanningResultBuilder.FileScanned(result, pair.Value);
            return ValueTask.CompletedTask;
        }).ConfigureAwait(false);
        return scanningResultBuilder.BuildResult();
    }

    public Task<ScanningResult> ExecuteAsync(IRequest<ScanningResult> request)
    {
        if (request is StartScanningRequest startScanningRequest) return ExecuteAsync(startScanningRequest);
        return Task.FromException<ScanningResult>(CreateExceptionOnInvalidRequestType());
    }

    private static InvalidOperationException CreateExceptionOnInvalidRequestType()
    {
        return new InvalidOperationException($"Unknown request type, expected request type {typeof(StartScanningRequest)}");
    }

    private ParallelOptions CreateParallelOptions(CancellationToken cancellationToken)
    {
        return new ParallelOptions { CancellationToken = cancellationToken, MaxDegreeOfParallelism = scanningOptions.Processors };
    }
}