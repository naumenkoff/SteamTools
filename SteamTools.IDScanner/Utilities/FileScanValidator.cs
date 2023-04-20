using SteamTools.Core.Models;

namespace SteamTools.IDScanner.Utilities;

public class FileScanValidator : IFileScanValidator
{
    private readonly SteamID32 _steam32ID;
    private readonly SteamID64 _steam64ID;

    public FileScanValidator(SteamID64 steamID64)
    {
        _steam64ID = steamID64;
        _steam32ID = steamID64.ToSteamID32();
    }

    public bool IsSteamIDPresentHere(string text)
    {
        return text.Contains(_steam32ID) || text.Contains(_steam64ID);
    }
}