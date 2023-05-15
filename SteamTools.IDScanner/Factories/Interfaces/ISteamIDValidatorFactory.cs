using SteamTools.Core.Models;
using SteamTools.IDScanner.Services.Interfaces;

namespace SteamTools.IDScanner.Factories.Interfaces;

public interface ISteamIDValidatorFactory
{
    IFileValidator Create(SteamID64 steamID64);
}