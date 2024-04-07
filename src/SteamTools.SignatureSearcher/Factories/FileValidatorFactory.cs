using SteamTools.Common;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Services;

namespace SteamTools.SignatureSearcher.Factories;

internal sealed class FileValidatorFactory : IFactory<ISteamIDPair, IFileValidator<string>>
{
    public IFileValidator<string> Create(ISteamIDPair arg)
    {
        return new FileValidator(arg);
    }
}