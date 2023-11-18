using System.Text.RegularExpressions;
using SteamTools.Domain.Providers;
using SteamTools.Domain.Services;
using SteamTools.Infrastructure.Services.LScanning;

namespace SteamTools.Infrastructure.Services;

public partial class ScannerPatternProvider : ITemplateProvider<IScanner>
{
    public Regex GetTemplate(IScanner value)
    {
        return value switch
        {
            AppmanifestScanner => AppmanifestPattern(),
            AppworkshopScanner => AppworkshopPattern(),
            ConfigScanner => ConfigPattern(),
            LoginusersScanner => LoginusersPattern(),
            _ => throw new ArgumentException("Unsupported scanner type.")
        };
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