namespace SteamTools.IDScanner.Models;

public class ScanningResult : IScanningResult, IScanningResultWriter
{
    private readonly List<string> _paths;

    private ScanningResult()
    {
        _paths = new List<string>();
    }

    public static ScanningResult Empty => new();

    public int TotalFiles { get; private set; }
    public int TotalScannedFiles { get; private set; }

    public List<string> GetResultSortedByLength()
    {
        return _paths.OrderBy(x => x.Length).ToList();
    }

    public void IncrementTotalFilesCount()
    {
        TotalFiles++;
    }

    public void IncrementTotalScannedFilesCount()
    {
        TotalScannedFiles++;
    }

    public void AddFilePath(string path)
    {
        _paths.Add(path);
    }
}