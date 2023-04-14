using System.Text.RegularExpressions;
using SteamTools.Core.Models;
using SteamTools.LocalProfileScanner.Models;
using SteamTools.LocalProfileScanner.Models.DataScanner;

namespace SteamTools.LocalProfileScanner.Services;

public partial class ProfileScannerService
{
    // TODO Create some class like Regex Pattern Provider and inject it into this class

    private readonly ISteamClient _steamClient;

    public ProfileScannerService(ISteamClient steamClient)
    {
        _steamClient = steamClient;
    }

    [GeneratedRegex(@"""appid""\s+""(\d+)""[\s\S]+?""name""\s+""([^""]+)""[\s\S]+?""LastOwner""\s+""(\d+)""")]
    private static partial Regex AppmanifestPattern();

    [GeneratedRegex(@"""appid""\s*""(\d+)""[\s\S]*?""subscribedby""\s*""(\d+)""")]
    private static partial Regex AppworkshopPattern();

    [GeneratedRegex("(\\w+)\"\\s*\\{\\s*\"SteamID\"\\s*\"(\\d+)\"")]
    private static partial Regex ConfigPattern();

    [GeneratedRegex(@"""(\w{17})""\s*\{(?:\s*""([^""]+)""\s*""([^""]+)""\s*)+\s*\}")]
    private static partial Regex LoginusersPattern();

    public void Execute()
    {
        var scanners = new List<IScanner>
        {
            new AppmanifestScanner(_steamClient, AppmanifestPattern()),
            new AppworkshopScanner(_steamClient, AppworkshopPattern()),
            new ConfigScanner(_steamClient, ConfigPattern()),
            new LoginusersScanner(_steamClient, LoginusersPattern()),
            new UserdataScanner(_steamClient),
            new RegistryScanner()
        }.AsParallel();

        var profiles = scanners.SelectMany(x => x.GetProfiles());
        foreach (var detectedProfile in profiles)
        {
            var profile = LocalProfile.GetAccount(detectedProfile);
            profile.Attach(detectedProfile);
        }
    }
}