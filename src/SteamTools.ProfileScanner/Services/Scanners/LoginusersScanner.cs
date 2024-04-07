using SProject.VDF;
using SProject.VDF.Extensions;
using SteamTools.Common;
using SteamTools.ProfileScanner.Abstractions;
using SteamTools.ProfileScanner.Models.ScanningResults;

namespace SteamTools.ProfileScanner.Services.Scanners;

internal sealed class LoginusersScanner(SteamClient steamClient) : IScanner
{
    public IEnumerable<ResultBase> EnumerateProfiles()
    {
        var file = steamClient.GetLoginusersFile();
        if (file is null) yield break;

        var content = ValveDataFileParser.Parse(file);
        if (!content.HasSections) yield break;

        foreach (var user in content.PrimarySection)
        {
            var profile = CreateSteamProfile(user.Key);
            yield return new LoginusersResult(profile)
            {
                Login = user.Properties.FirstOrDefault("PersonaName")?.Value,
                Name = user.Properties.FirstOrDefault("AccountName")?.Value,
                Timestamp = user.Properties.FirstOrDefault("Timestamp").AsDateTimeOffset()
            };
        }
    }

    private static SteamProfile CreateSteamProfile(string key)
    {
        var id = long.Parse(key);
        return new SteamProfile(id);
    }
}