using SteamTools.Domain.Models;
using SteamTools.Domain.Services;

namespace SteamTools.Domain.Factories;

public interface IScanningServiceFactory
{
    IScanningService Create(SteamProfile steamProfile, CancellationToken cancellationToken);
}