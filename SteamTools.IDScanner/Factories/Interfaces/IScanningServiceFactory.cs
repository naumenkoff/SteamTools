using SteamTools.Core.Models;
using SteamTools.IDScanner.Services.Interfaces;

namespace SteamTools.IDScanner.Factories.Interfaces;

public interface IScanningServiceFactory
{
    IScanningService Create(SteamID64 steamID64, bool limitMaximumFileSize,
        long maximumFileSizeInBytes, bool useSpecifiedExtensions, int processorCount,
        CancellationToken cancellationToken, params string[] extensions);
}