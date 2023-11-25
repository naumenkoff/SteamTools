using SteamTools.Domain.Models;

namespace SteamTools.SignatureSearcher.Abstractions;

public interface IScanningServiceFactory
{
    IScanningService Create(SteamProfile steamProfile, CancellationToken cancellationToken);
}