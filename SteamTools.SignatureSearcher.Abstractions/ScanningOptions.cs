using SProject.Math;

namespace SteamTools.SignatureSearcher.Abstractions;

public class ScanningOptions
{
    private int _maximumFileSize;

    public ScanningOptions()
    {
        TotalProcessors = Environment.ProcessorCount;
        Processors = Math.Max(TotalProcessors / 2, 1);
        LimitScanningFileSize = true;
        MaximumFileSize = 1;
        Extensions = new HashSet<string>();
    }

    public bool ScanFilesOnlyWithSpecifiedExtensions { get; set; }
    public int Processors { get; set; }
    public int TotalProcessors { get; }

    public int MaximumFileSize
    {
        get => _maximumFileSize;
        set => _maximumFileSize = Math.Min(Math.Max(0, value), 1024);
    }

    public bool LimitScanningFileSize { get; set; }
    public bool IsScanning { get; set; }

    public HashSet<string> Extensions { get; set; }

    public long GetFormattedMaximumFileSize()
    {
        return LimitScanningFileSize ? ByteUnitConverter.MegabytesToBytes(MaximumFileSize) : 0;
    }
}