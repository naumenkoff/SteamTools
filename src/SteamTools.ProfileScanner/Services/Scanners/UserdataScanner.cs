using SteamTools.Common;
using SteamTools.ProfileScanner.Abstractions;
using SteamTools.ProfileScanner.Enums;
using SteamTools.ProfileScanner.Models.ScanningResults;

namespace SteamTools.ProfileScanner.Services.Scanners;

internal sealed class UserdataScanner(SteamClient steamClient) : IScanner
{
    public IEnumerable<ResultBase> EnumerateProfiles()
    {
        var userdata = steamClient.Steam?.GetUserdataDirectory();
        if (userdata is null) yield break;

        foreach (var profile in EnumerateSteamIDs(userdata).Select(x => new SteamProfile(x)))
            yield return new ResultBase(profile, ResultType.Userdata);
    }

    private static IEnumerable<uint> EnumerateSteamIDs(DirectoryInfo parentDirectoryInfo)
    {
        foreach (var childDirectoryInfo in parentDirectoryInfo.EnumerateDirectories())
            if (uint.TryParse(childDirectoryInfo.Name, out var id))
                yield return id;
    }
}