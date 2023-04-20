namespace SteamTools.IDScanner.Models;

public interface IFileScanner
{
    void ScanFile(FileInfo file, CancellationToken token);
}