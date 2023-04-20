namespace SteamTools.IDScanner.Models;

public interface IScanningResult
{
    int TotalFiles { get; }
    int TotalScannedFiles { get; }
    List<string> GetResultSortedByLength();
}