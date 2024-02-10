using System.Diagnostics.CodeAnalysis;
using SteamTools.Common;

namespace SteamTools.SignatureSearcher;

internal class FileScanner : IFileScanner
{
    private readonly Func<ISteamIDPair, IFileValidator> _fileValidatorFactory;
    private readonly bool _isFileSizeLimitEnabled;
    private readonly long _maximumFileSize;
    private readonly IScanningResultWriter _scanningResult;
    private IFileValidator _fileValidator = null!;

    public FileScanner(IScanningResultWriter scanningResult, ScanningOptions scanningOptions,
        Func<ISteamIDPair, IFileValidator> fileValidatorFactory)
    {
        _fileValidatorFactory = fileValidatorFactory;
        _isFileSizeLimitEnabled = scanningOptions.LimitScanningFileSize;
        _maximumFileSize = scanningOptions.GetFormattedMaximumFileSize();
        _scanningResult = scanningResult;
    }

    [MemberNotNull(nameof(_fileValidator))]
    public void Initialize(ISteamIDPair steamIDPair)
    {
        _fileValidator = _fileValidatorFactory(steamIDPair);
    }

    public async Task ScanFile(FileInfo? file, CancellationToken token)
    {
        _scanningResult.MarkScannedFile();
        if (file is null) return;

        if (_isFileSizeLimitEnabled && file.Length > _maximumFileSize) return;

        try
        {
            using var streamReader = file.OpenText();

            if (token.IsCancellationRequested) return;
            token.ThrowIfCancellationRequested();

            _scanningResult.MarkSuccessfullyScannedFile();
            while (streamReader.BaseStream.Position < streamReader.BaseStream.Length)
            {
                var line = await streamReader.ReadLineAsync(token).ConfigureAwait(false);
                if (!_fileValidator.ContainsSteamId(line)) continue;

                _scanningResult.AddFilePath(file.FullName);
                return;
            }
        }
        catch
        {
            // ignore
        }
    }

    public IScanningResult GetResult()
    {
        return (IScanningResult)_scanningResult;
    }
}