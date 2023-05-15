using SteamTools.IDScanner.Models;
using SteamTools.IDScanner.Services.Interfaces;

namespace SteamTools.IDScanner.Factories.Interfaces;

public interface IFileScannerFactory
{
    IFileScanner Create(IScanningResultWriter scanningResultWriter, IFileValidator fileValidator,
        bool limitFileSize, long maximumFileSize);
}