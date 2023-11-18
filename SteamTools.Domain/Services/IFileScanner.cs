namespace SteamTools.Domain.Services;

public interface IFileScanner
{
    Task ScanFile(FileInfo? file, CancellationToken token);
}