using System.Diagnostics.CodeAnalysis;
using SteamTools.Common;

namespace SteamTools.SignatureSearcher.Abstractions;

internal abstract class FileProviderBase(ScanningOptions scanningOptions)
{
    private readonly bool _isFileSizeLimitEnabled = scanningOptions.LimitScanningFileSize;
    private readonly long _maximumFileSize = scanningOptions.GetFormattedMaximumFileSize();

    public IEnumerable<FileInfo> EnumerateSuitableFiles()
    {
        return EnumerateFiles().Where(IsFileMeetCriteria);
    }

    private bool IsFileMeetCriteria([NotNullWhen(true)] FileInfo? fileInfo)
    {
        if (fileInfo is not { Exists: true }) return false;
        return !_isFileSizeLimitEnabled || fileInfo.Length <= _maximumFileSize;
    }

    protected abstract IEnumerable<FileInfo> EnumerateFiles();
}