using SteamTools.Domain.Models;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services;

public class FileScanner : IFileScanner
{
    private readonly IFileValidator _fileValidator;
    private readonly bool _isFileSizeLimitEnabled;
    private readonly long _maximumFileSize;
    private readonly IScanningResultWriter _scanningResultWriter;

    public FileScanner(IScanningResultWriter scanningResultWriter, IFileValidator fileValidator, bool isFileSizeLimitEnabled, long maximumFileSize)
    {
        _fileValidator = fileValidator;
        _isFileSizeLimitEnabled = isFileSizeLimitEnabled;
        _maximumFileSize = maximumFileSize;
        _scanningResultWriter = scanningResultWriter;
    }

    public async Task ScanFile(FileInfo? file, CancellationToken token)
    {
        _scanningResultWriter.MarkScannedFile();
        if (file is null) return;

        if (_isFileSizeLimitEnabled && file.Length > _maximumFileSize) return;

        try
        {
            using var streamReader = file.OpenText();

            if (token.IsCancellationRequested) return;
            token.ThrowIfCancellationRequested();

            _scanningResultWriter.MarkSuccessfullyScannedFile();
            while (!streamReader.EndOfStream)
            {
                var line = await streamReader.ReadLineAsync(token);
                if (!_fileValidator.ContainsSteamID(line)) continue;

                _scanningResultWriter.AddFilePath(file.FullName);
                return;
            }
        }
        catch
        {
            // ignore
        }
    }
}