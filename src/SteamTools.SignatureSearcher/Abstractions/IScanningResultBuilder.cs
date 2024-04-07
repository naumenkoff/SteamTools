using SteamTools.SignatureSearcher.Contracts.Responses;
using SteamTools.SignatureSearcher.Enums;

namespace SteamTools.SignatureSearcher.Abstractions;

public interface IScanningResultBuilder
{
    void FileScanned(FileScanResult fileScanResult, FileInfo? fileInfo);
    ScanningResult BuildResult();
}