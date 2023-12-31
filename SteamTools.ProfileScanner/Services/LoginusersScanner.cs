using SProject.VDF;
using SteamTools.Domain.Models;
using SteamTools.Domain.Services;
using SteamTools.ProfileScanner.Abstractions;

namespace SteamTools.ProfileScanner.Services;

public class LoginusersScanner : IScanner
{
    private readonly SteamClient _steamClient;

    public LoginusersScanner(SteamClient steamClient)
    {
        _steamClient = steamClient;
    }

    public IEnumerable<LocalResult> GetProfiles()
    {
        var file = _steamClient.GetLoginusersFile();
        if (file is null) yield break;

        var content = ByteVdfParser.Parse(file);
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