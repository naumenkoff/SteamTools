using System.Diagnostics.CodeAnalysis;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Contracts.Responses;
using SteamTools.SignatureSearcher.Enums;

namespace SteamTools.SignatureSearcher.Services;

internal sealed class ScanningResultBuilder : IScanningResultBuilder
{
    private readonly List<string> _files = [];
    private readonly object _lock = new();
    private int _errors;
    private int _openedFiles;
    private int _scannedFiles;

    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeEnumCasesNoDefault")]
    public void FileScanned(FileScanResult fileScanResult, FileInfo? fileInfo)
    {
        Interlocked.Increment(ref _scannedFiles);
        switch (fileScanResult)
        {
            case FileScanResult.NotFound:
            {
                Interlocked.Increment(ref _openedFiles);
                break;
            }
            case FileScanResult.Detected:
            {
                Interlocked.Increment(ref _openedFiles);
                lock (_lock)
                {
                    _files.Add(fileInfo!.FullName);
                }

                break;
            }
            case FileScanResult.Failed:
            {
                Interlocked.Increment(ref _errors);
                break;
            }
        }
    }

    public ScanningResult BuildResult()
    {
        lock (_lock)
        {
            return new ScanningResult
            {
                ScannedFiles = _scannedFiles,
                OpenedFiles = _openedFiles,
                Files = _files
            };
        }
    }
}