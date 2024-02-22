using SteamTools.Common;

namespace SteamTools.SignatureSearcher;

internal sealed class FileValidator : IFileValidator
{
    private readonly string _steam32Id;
    private readonly string _steam64Id;

    public FileValidator(ISteamIDPair steamIdPair)
    {
        _steam64Id = steamIdPair.ID64.AsString;
        _steam32Id = steamIdPair.ID32.AsString;
    }

    public bool ContainsSteamId(string? value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        return value.Contains(_steam64Id, StringComparison.OrdinalIgnoreCase) || value.Contains(_steam32Id, StringComparison.OrdinalIgnoreCase);
    }
}