using SProject.FileSystem;
using SteamTools.Common;
using SteamTools.SignatureSearcher.Abstractions;

namespace SteamTools.SignatureSearcher.Services;

internal sealed class FileProvider(SteamClient steamClient, ScanningOptions scanningOptions) : FileProviderBase(scanningOptions)
{
    protected override IEnumerable<FileInfo> EnumerateFiles()
    {
        if (steamClient.Steam is null) yield break;
        foreach (var steamLibrary in steamClient.Steam.GetSteamLibraries())
        foreach (var file in steamLibrary.WorkingDirectory.EnumerateAllFiles())
            yield return file;
    }
}