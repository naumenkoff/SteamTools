using SProject.FileSystem;
using SteamTools.Common;
using SteamTools.SignatureSearcher.Abstractions;

namespace SteamTools.SignatureSearcher.Services;

internal sealed class CertainFileProvider(SteamClient steamClient, ScanningOptions scanningOptions) : FileProviderBase(scanningOptions)
{
    private readonly ScanningOptions _scanningOptions = scanningOptions;

    protected override IEnumerable<FileInfo> EnumerateFiles()
    {
        if (steamClient.Steam is null) yield break;
        if (_scanningOptions.Extensions.Count == 0) yield break;
        var extensions = _scanningOptions.Extensions;
        foreach (var steamLibrary in steamClient.Steam.GetSteamLibraries())
        foreach (var file in steamLibrary.WorkingDirectory.EnumerateAllFiles())
            if (extensions.Contains(file.Extension))
                yield return file;
    }
}