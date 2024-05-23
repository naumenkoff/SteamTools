using SteamTools.SignatureSearcher.Enums;

namespace SteamTools.SignatureSearcher.Abstractions;

public abstract class FileScannerBase
{
    public abstract FileScanResult ScanFile(FileInfo fileInfo, CancellationToken cancellationToken = default);

    protected static class FileStreamOptionsFactory
    {
        private const int DefaultBufferSize = 4096;

        private static readonly Lazy<FileStreamOptions> DefaultFileStreamOptions = new(new FileStreamOptions
        {
            BufferSize = DefaultBufferSize,
            Access = FileAccess.Read,
            Mode = FileMode.Open
        });

        public static FileStreamOptions CreateFileStreamOptions(FileInfo fileInfo, out int size)
        {
            if (fileInfo.Length >= DefaultBufferSize)
            {
                size = DefaultBufferSize;
                return DefaultFileStreamOptions.Value;
            }

            size = (int)fileInfo.Length;
            return new FileStreamOptions
            {
                BufferSize = size,
                Access = FileAccess.Read,
                Mode = FileMode.Open
            };
        }
    }
}