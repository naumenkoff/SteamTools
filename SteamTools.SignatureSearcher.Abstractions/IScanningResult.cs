namespace SteamTools.SignatureSearcher.Abstractions;

public interface IScanningResult
{
    int TotalScannedFiles { get; }
    int SuccessfullyScannedFiles { get; }
    IOrderedEnumerable<string> GetResultSortedByLength();
}