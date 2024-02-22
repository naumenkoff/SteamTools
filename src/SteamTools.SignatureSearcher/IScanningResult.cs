namespace SteamTools.SignatureSearcher;

public interface IScanningResult
{
    int TotalScannedFiles { get; }
    int SuccessfullyScannedFiles { get; }
    IOrderedEnumerable<string> GetResultSortedByLength();
}