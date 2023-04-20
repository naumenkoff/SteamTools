using System.Collections.Concurrent;

namespace SteamTools.IDScanner.Models;

public class ScanningResult : IScanningResult, IScanningResultWriter
{
    private readonly ConcurrentBag<string> _paths;

    public ScanningResult()
    {
        _paths = new ConcurrentBag<string>();
    }

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