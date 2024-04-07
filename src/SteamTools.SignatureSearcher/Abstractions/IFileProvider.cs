namespace SteamTools.SignatureSearcher.Abstractions;

public interface IFileProvider
{
    IEnumerable<FileInfo> EnumerateSuitableFiles();
}