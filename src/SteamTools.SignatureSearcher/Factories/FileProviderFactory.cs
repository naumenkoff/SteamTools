using SteamTools.Common;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Services;

namespace SteamTools.SignatureSearcher.Factories;

internal sealed class FileProviderFactory(
    Func<CertainFileProvider> allowedFileProvider,
    Func<FileProvider> fileProvider,
    ScanningOptions scanningOptions) : IFactory<FileProviderBase>
{
    public FileProviderBase Create()
    {
        return scanningOptions.ScanFilesOnlyWithSpecifiedExtensions ? allowedFileProvider() : fileProvider();
    }
}