using SteamTools.IDScanner.Factories.Interfaces;
using SteamTools.IDScanner.Models;
using SteamTools.IDScanner.Services.Implementations;
using SteamTools.IDScanner.Services.Interfaces;

namespace SteamTools.IDScanner.Factories.Implementations;

public class FileScannerFactory : IFileScannerFactory
{
    public IFileScanner Create(IScanningResultWriter scanningResultWriter, IFileValidator fileValidator,
        bool limitFileSize, long maximumFileSize)
    {
        return new FileScanner(scanningResultWriter, fileValidator, limitFileSize, maximumFileSize);
    }
}