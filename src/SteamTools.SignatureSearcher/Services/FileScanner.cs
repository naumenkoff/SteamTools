using System.Text;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Enums;

namespace SteamTools.SignatureSearcher.Services;

internal class FileScanner(IFileValidator<string> fileValidator) : IFileScanner
{
    private const int DefaultBufferSize = 4096;

    public FileScanResult ScanFile(FileInfo fileInfo, CancellationToken cancellationToken)
    {
        try
        {
            using var streamReader = CreateStreamReader(fileInfo);
            if (cancellationToken.IsCancellationRequested) return FileScanResult.Cancelled;
            while (streamReader.BaseStream.Position < streamReader.BaseStream.Length)
            {
                var line = streamReader.ReadLine();
                if (fileValidator.Validate(line)) return FileScanResult.Detected;
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
        var fileStreamOptions = new FileStreamOptions
        {
            BufferSize = fileInfo.Length >= DefaultBufferSize ? DefaultBufferSize : (int)fileInfo.Length,
            Access = FileAccess.Read,
            Mode = FileMode.Open
        };

        return new StreamReader(fileInfo.FullName, Encoding.UTF8, true, fileStreamOptions);
    }
}