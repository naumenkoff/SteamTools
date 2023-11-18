using SteamTools.Domain.Models;

namespace SteamTools.Domain.Services;

public interface IScanningService
{
    ValueTask<IScanningResult> StartScanningAsync();
}