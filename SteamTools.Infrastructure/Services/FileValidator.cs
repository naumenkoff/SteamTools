using SteamTools.Domain.Models;
using SteamTools.Domain.Services;

namespace SteamTools.Infrastructure.Services;

public class FileValidator : IFileValidator
{
    private readonly string _steam32ID;
    private readonly string _steam64ID;

    public FileValidator(ISteamIDPair steamIDPair)
    {
        _steam64ID = steamIDPair.ID64.AsString;
        _steam32ID = steamIDPair.ID32.AsString;
    }

    public bool ContainsSteamID(string? value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        return value.Contains(_steam64ID) || value.Contains(_steam32ID);
    }
}