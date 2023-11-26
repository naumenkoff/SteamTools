using SteamTools.Domain.Models;

namespace SteamTools.SignatureSearcher.Abstractions;

public interface IFileScanner
{
    Task ScanFile(FileInfo? file, CancellationToken token);
    void Initialize(ISteamIDPair steamIDPair);
    IScanningResult GetResult();
}