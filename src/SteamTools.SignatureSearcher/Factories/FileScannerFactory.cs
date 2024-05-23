using SteamTools.Common;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Services;

namespace SteamTools.SignatureSearcher.Factories;

internal sealed class FileScannerFactory : IFactory<ISteamIDPair, FileScannerBase>
{
    public FileScannerBase Create(ISteamIDPair arg)
    {
        return new BinaryFileScanner(arg);
    }
}