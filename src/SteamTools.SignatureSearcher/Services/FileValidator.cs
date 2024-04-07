using SteamTools.Common;
using SteamTools.SignatureSearcher.Abstractions;

namespace SteamTools.SignatureSearcher.Services;

internal class FileValidator(ISteamIDPair steamIdPair) : IFileValidator<string>
{
    private readonly string _id32 = steamIdPair.ID32.AsString;
    private readonly string _id64 = steamIdPair.ID64.AsString;

    public bool Validate(string? value)
    {
        if (value is null) return false;
        return value.Contains(_id32, StringComparison.OrdinalIgnoreCase)
               || value.Contains(_id64, StringComparison.OrdinalIgnoreCase);
    }
}