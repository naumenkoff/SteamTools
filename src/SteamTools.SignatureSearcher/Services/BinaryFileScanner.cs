using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

            var id64Position = 0;
            var id32Position = 0;
            buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
            while (fileStream.Position < fileStream.Length)
            {
                var read = fileStream.Read(buffer, 0, buffer.Length);
                ref var start = ref MemoryMarshal.GetArrayDataReference(buffer);
                ref var end = ref Unsafe.Add(ref start, read);
                for (; Unsafe.IsAddressLessThan(ref start, ref end); start = ref Unsafe.Add(ref start, 1))
                {
                    if (IsContains(_id32, ref id32Position, start) || IsContains(_id64, ref id64Position, start)) return FileScanResult.Detected;
                    if (cancellationToken.IsCancellationRequested) return FileScanResult.Cancelled;
                }
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsContains(byte[] array, ref int position, byte value)
    {
        if (array[position] == value)
            return ++position == array.Length;
        position = 0;
        return false;
    }

    private static FileStream CreateStreamReader(FileInfo fileInfo, out int bufferSize)
    {
        var fileStreamOptions = FileStreamOptionsFactory.CreateFileStreamOptions(fileInfo, out bufferSize);
        return new FileStream(fileInfo.FullName, fileStreamOptions);
    }
}