using System.Buffers;
using System.Text;
using SteamTools.Common;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Enums;

namespace SteamTools.SignatureSearcher.Services;

internal sealed class BinaryFileScanner(ISteamIDPair steamIdPair) : FileScannerBase
{
    private readonly byte[] _id32 = Encoding.UTF8.GetBytes(steamIdPair.ID32.AsString);
    private readonly byte[] _id64 = Encoding.UTF8.GetBytes(steamIdPair.ID64.AsString);

    public override FileScanResult ScanFile(FileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        byte[]? buffer = null;
        FileStream? fileStream = null;

        try
        {
            fileStream = CreateStreamReader(fileInfo, out var bufferSize);
            if (cancellationToken.IsCancellationRequested) return FileScanResult.Cancelled;

            buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
            while (fileStream.Position < fileStream.Length)
            {
                var read = fileStream.Read(buffer, 0, buffer.Length);
                var span = buffer.AsSpan(0, read);
                if (span.IndexOf(_id64) != -1) return FileScanResult.Detected;
                if (span.IndexOf(_id32) != -1) return FileScanResult.Detected;
                if (cancellationToken.IsCancellationRequested) return FileScanResult.Cancelled;
            }

            return FileScanResult.NotFound;
        }
        catch
        {
            return FileScanResult.Failed;
        }
        finally
        {
            if (fileStream != null) fileStream.Dispose();
            if (buffer != null) ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private static FileStream CreateStreamReader(FileInfo fileInfo, out int bufferSize)
    {
        var fileStreamOptions = FileStreamOptionsFactory.CreateFileStreamOptions(fileInfo, out bufferSize);
        return new FileStream(fileInfo.FullName, fileStreamOptions);
    }
}