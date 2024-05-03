using System.Text;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Enums;

namespace SteamTools.SignatureSearcher.Services;

internal class FileScanner(IFileValidator<string> fileValidator) : IFileScanner
{
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
        var fileStreamOptions = FileStreamOptionsFactory.CreateFileStreamOptions(fileInfo);
        return new StreamReader(fileInfo.FullName, Encoding.UTF8, true, fileStreamOptions);
    }

    private static class FileStreamOptionsFactory
    {
        private const int DefaultBufferSize = 4096;

        private static readonly Lazy<FileStreamOptions> DefaultFileStreamOptions = new(new FileStreamOptions
        {
            BufferSize = DefaultBufferSize,
            Access = FileAccess.Read,
            Mode = FileMode.Open
        });

        public static FileStreamOptions CreateFileStreamOptions(FileInfo fileInfo)
        {
            return fileInfo.Length >= DefaultBufferSize
                ? DefaultFileStreamOptions.Value
                : new FileStreamOptions
                {
                    BufferSize = (int)fileInfo.Length,
                    Access = FileAccess.Read,
                    Mode = FileMode.Open
                };
        }
    }
}