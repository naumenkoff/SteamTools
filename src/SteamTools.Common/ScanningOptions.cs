using CommunityToolkit.Mvvm.ComponentModel;
using SProject.Math;

namespace SteamTools.Common;

public class ScanningOptions : ObservableObject
{
    private bool _limitScanningFileSize;
    private int _maximumFileSize;

    private int _processors;

    private bool _scanFilesOnlyWithSpecifiedExtensions;

    public ScanningOptions()
    {
        TotalProcessors = Environment.ProcessorCount;
        Processors = Math.Max(TotalProcessors / 2, 1);
        LimitScanningFileSize = true;
        MaximumFileSize = 1;
        Extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    public bool ScanFilesOnlyWithSpecifiedExtensions
    {
        get => _scanFilesOnlyWithSpecifiedExtensions;
        set
        {
            _scanFilesOnlyWithSpecifiedExtensions = value;
            OnPropertyChanged();
        }
    }

    public int Processors
    {
        get => _processors;
        set
        {
            _processors = value;
            OnPropertyChanged();
        }
    }

    public int TotalProcessors { get; }

    public int MaximumFileSize
    {
        get => _maximumFileSize;
        set
        {
            _maximumFileSize = Math.Min(Math.Max(0, value), 1024);
            OnPropertyChanged();
        }
    }

    public bool LimitScanningFileSize
    {
        get => _limitScanningFileSize;
        set
        {
            _limitScanningFileSize = value;
            OnPropertyChanged();
        }
    }

    public bool IsScanning { get; set; }

    public HashSet<string> Extensions { get; set; }

    public long GetFormattedMaximumFileSize()
    {
        return LimitScanningFileSize ? ByteUnitConverter.MegabytesToBytes(MaximumFileSize) : 0;
    }
}