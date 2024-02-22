using SProject.VDF;
using SteamTools.Common;

namespace SteamTools.ProfileScanner;

internal sealed class LoginusersScanner(SteamClient steamClient) : IScanner
{
    public IEnumerable<LocalResult> GetProfiles()
    {
        var file = steamClient.GetLoginusersFile();
        if (file is null) yield break;

        var content = VParser.Parse(file);
        if (content.Root is null) yield break;

        foreach (var user in content.Root)
        {
            var id64 = long.Parse(user.Key);
            var profile = new SteamProfile(id64);
            yield return new LoginusersData(profile, LocalResultType.Loginusers)
            {
                Login = user.GetValue("PersonaName")?.Value,
                Name = user.GetValue("AccountName")?.Value,
                Timestamp = user.GetValue("Timestamp").AsDateTimeOffset()
            };
        }
    }
}