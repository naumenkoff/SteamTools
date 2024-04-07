using SteamTools.ProfileScanner.Models.ScanningResults;

namespace SteamTools.ProfileScanner.Abstractions;

public interface IScanner
{
    IEnumerable<ResultBase> EnumerateProfiles();
}