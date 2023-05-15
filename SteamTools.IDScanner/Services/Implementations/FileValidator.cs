using SteamTools.Core.Models;
using SteamTools.IDScanner.Services.Interfaces;

namespace SteamTools.IDScanner.Services.Implementations;

public class FileValidator : IFileValidator
{
    private readonly string _steam32ID;
    private readonly string _steam64ID;

    public FileValidator(SteamID64 steamID64)
    {
        _steam64ID = steamID64.AsString;
        _steam32ID = steamID64.ToSteamID32().AsString;
    }

    public bool ContainsSteamID(string value)
    {
        return value.Contains(_steam64ID) || value.Contains(_steam32ID);
    }
}