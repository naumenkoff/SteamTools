namespace SteamTools.IDScanner.Models;

public interface IScanningResultWriter
{
    void AddFilePath(string path);
    void IncrementTotalFilesCount();
    void IncrementTotalScannedFilesCount();
}