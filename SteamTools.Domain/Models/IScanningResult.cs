namespace SteamTools.Domain.Models;

public interface IScanningResult
{
    int TotalScannedFiles { get; }
    int SuccessfullyScannedFiles { get; }
    IOrderedEnumerable<string> GetResultSortedByLength();
}