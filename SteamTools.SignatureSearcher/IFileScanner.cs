using SteamTools.Common;

namespace SteamTools.SignatureSearcher;

public interface IFileScanner
{
    Task ScanFile(FileInfo? file, CancellationToken token);
    void Initialize(ISteamIDPair steamIDPair);
    IScanningResult GetResult();
}