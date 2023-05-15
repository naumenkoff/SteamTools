namespace SteamTools.IDScanner.Services.Interfaces;

public interface IFileScanner
{
    void ScanFile(FileInfo file, CancellationToken token);
}