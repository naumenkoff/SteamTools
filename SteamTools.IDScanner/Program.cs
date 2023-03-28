using System.Diagnostics;
using SteamTools.IDScanner.Factories;

namespace SteamTools.IDScanner;

public class Program
{
    public static void Main()
    {
        var scanningService = ScanningServiceFactory.GetScanningService();
        var startingTimestamp = Stopwatch.GetTimestamp();
        scanningService.StartScanning();
        Console.WriteLine($"The scan is completed in {Stopwatch.GetElapsedTime(startingTimestamp).TotalSeconds} sec.");
        Console.Write("The program has finished working. Press any key to exit.");
        Console.ReadKey();
    }
}