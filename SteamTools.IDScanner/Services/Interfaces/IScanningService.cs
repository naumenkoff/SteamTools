using SteamTools.IDScanner.Models;

namespace SteamTools.IDScanner.Services.Interfaces;

public interface IScanningService
{
    Task<IScanningResult> StartScanningAsync();
}