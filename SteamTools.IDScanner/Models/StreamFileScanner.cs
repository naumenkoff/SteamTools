using SteamTools.Core.Models;
using SteamTools.IDScanner.Utilities;

namespace SteamTools.IDScanner.Models;

public class StreamFileScanner : IFileScanner
{
    private readonly IFileScanValidator _fileScanValidator;
    private readonly bool _limitFileSize;
    private readonly long _maximumFileSize;
    private readonly IScanningResultWriter _scanningResultWriter;

    public StreamFileScanner(IScanningResultWriter scanningResultWriter, SteamID64 steamID64, bool limitFileSize,
        long maximumFileSize)
    {
        _fileScanValidator = new FileScanValidator(steamID64);
        _limitFileSize = limitFileSize;
        _maximumFileSize = maximumFileSize;
        _scanningResultWriter = scanningResultWriter;
    }

    public void ScanFile(FileInfo file, CancellationToken token)
    {
        _scanningResultWriter.IncrementTotalFilesCount();
        if (file is null) return;
        if (_limitFileSize && file.Length > _maximumFileSize) return;
        try
        {
            using var streamReader = file.OpenText();

            if (token.IsCancellationRequested) return;
            token.ThrowIfCancellationRequested();

            _scanningResultWriter.IncrementTotalScannedFilesCount();
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (_fileScanValidator.IsSteamIDPresentHere(line) is false) continue;
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