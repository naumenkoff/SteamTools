using SteamTools.SignatureSearcher.Enums;

namespace SteamTools.SignatureSearcher.Abstractions;

public interface IFileScanner
{
    FileScanResult ScanFile(FileInfo fileInfo, CancellationToken cancellationToken = default);
}