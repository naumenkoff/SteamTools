using System.Text;
using SteamTools.Common;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Enums;

namespace SteamTools.SignatureSearcher.Services;

internal sealed class FileScanner(ISteamIDPair steamIdPair) : FileScannerBase
{
    private readonly string _id32 = steamIdPair.ID32.AsString;
    private readonly string _id64 = steamIdPair.ID64.AsString;

    private bool Validate(string? value)
    {
        if (value is null) return false;
        return value.Contains(_id32, StringComparison.OrdinalIgnoreCase)
               || value.Contains(_id64, StringComparison.OrdinalIgnoreCase);
    }

    public override FileScanResult ScanFile(FileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        try
        {
            using var streamReader = CreateStreamReader(fileInfo);
            if (cancellationToken.IsCancellationRequested) return FileScanResult.Cancelled;
            while (streamReader.BaseStream.Position < streamReader.BaseStream.Length)
            {
                var line = streamReader.ReadLine();
                if (Validate(line)) return FileScanResult.Detected;
                if (cancellationToken.IsCancellationRequested) return FileScanResult.Cancelled;
            }

            return FileScanResult.NotFound;
        }
        catch
        {
            return FileScanResult.Failed;
        }
    }

    private static StreamReader CreateStreamReader(FileInfo fileInfo)
    {
        var fileStreamOptions = FileStreamOptionsFactory.CreateFileStreamOptions(fileInfo, out _);
        return new StreamReader(fileInfo.FullName, Encoding.UTF8, true, fileStreamOptions);
    }
}