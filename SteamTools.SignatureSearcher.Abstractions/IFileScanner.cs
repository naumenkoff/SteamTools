namespace SteamTools.SignatureSearcher.Abstractions;

public interface IFileScanner
{
    Task ScanFile(FileInfo? file, CancellationToken token);
}