using SteamTools.Core.Models;
using SteamTools.IDScanner.Factories.Interfaces;
using SteamTools.IDScanner.Services.Implementations;
using SteamTools.IDScanner.Services.Interfaces;

namespace SteamTools.IDScanner.Factories.Implementations;

public class SteamIDValidatorFactory : ISteamIDValidatorFactory
{
    public IFileValidator Create(SteamID64 steamID64)
    {
        return new FileValidator(steamID64);
    }
}