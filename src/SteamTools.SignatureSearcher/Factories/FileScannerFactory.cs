using SteamTools.Common;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Services;

namespace SteamTools.SignatureSearcher.Factories;

internal sealed class FileScannerFactory(IFactory<ISteamIDPair, IFileValidator<string>> fileValidatorFactory)
    : IFactory<ISteamIDPair, IFileScanner>
{
    public IFileScanner Create(ISteamIDPair arg)
    {
        var fileValidator = fileValidatorFactory.Create(arg);
        return new FileScanner(fileValidator);
    }
}