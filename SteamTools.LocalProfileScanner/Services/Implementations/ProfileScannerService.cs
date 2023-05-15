using System.Text.RegularExpressions;
using SteamTools.Core.Models.Steam;
using SteamTools.LocalProfileScanner.Models;
using SteamTools.LocalProfileScanner.Services.Interfaces;

namespace SteamTools.LocalProfileScanner.Services.Implementations;

public partial class ProfileScannerService : IProfileScannerService
{
    private readonly ILocalProfileStorage _localProfileStorage;
    private readonly ISteamClient _steamClient;

    public ProfileScannerService(ISteamClient steamClient, ILocalProfileStorage localProfileStorage)
    {
        _steamClient = steamClient;
        _localProfileStorage = localProfileStorage;
    }

    public async Task ExecuteAsync()
    {
        await Task.Run(() =>
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

            var profiles = scanners.SelectMany(x => x.GetProfiles().ToList());
            foreach (var detectedProfile in profiles)
            {
                var profile = _localProfileStorage.GetAccount(detectedProfile);
                profile.Attach(detectedProfile);
            }
        });
    }

    [GeneratedRegex(@"""appid""\s+""(\d+)""[\s\S]+?""name""\s+""([^""]+)""[\s\S]+?""LastOwner""\s+""(\d+)""")]
    private static partial Regex AppmanifestPattern();

    [GeneratedRegex(@"""appid""\s*""(\d+)""[\s\S]*?""subscribedby""\s*""(\d+)""")]
    private static partial Regex AppworkshopPattern();

    [GeneratedRegex("(\\w+)\"\\s*\\{\\s*\"SteamID\"\\s*\"(\\d+)\"")]
    private static partial Regex ConfigPattern();

    [GeneratedRegex(@"""(\w{17})""\s*\{(?:\s*""([^""]+)""\s*""([^""]+)""\s*)+\s*\}")]
    private static partial Regex LoginusersPattern();
}