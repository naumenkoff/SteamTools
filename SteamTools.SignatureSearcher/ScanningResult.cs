namespace SteamTools.SignatureSearcher;

public class ScanningResult : IScanningResult, IScanningResultWriter
{
    private readonly List<string> _paths = [];
    private int _successfullyScannedFiles;
    private int _totalScannedFiles;

    public int TotalScannedFiles => _totalScannedFiles;
    public int SuccessfullyScannedFiles => _successfullyScannedFiles;

    public IOrderedEnumerable<string> GetResultSortedByLength()
    {
        return _paths.OrderBy(x => x.Length);
    }

    public void MarkScannedFile()
    {
        Interlocked.Increment(ref _totalScannedFiles);
    }

    public void MarkSuccessfullyScannedFile()
    {
        Interlocked.Increment(ref _successfullyScannedFiles);
    }

    public void AddFilePath(string path)
    {
        _paths.Add(path);
    }
}