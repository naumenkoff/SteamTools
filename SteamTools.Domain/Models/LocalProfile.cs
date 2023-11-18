using System.Text;
using SProject.Steam;
using SteamTools.Domain.Models.LScanning;

namespace SteamTools.Domain.Models;

public class LocalProfile : ISteamIDPair
{
    private readonly List<AppmanifestData> _appmanifest = new List<AppmanifestData>();

    private readonly List<AppworkshopData> _appworkshop = new List<AppworkshopData>();

    private readonly List<ConfigData> _config = new List<ConfigData>();

    private readonly List<LoginusersData> _loginusers = new List<LoginusersData>();

    private readonly List<RegistryData> _registry = new List<RegistryData>();

    private readonly List<UserdataData> _userdata = new List<UserdataData>();

    public LocalProfile(ISteamIDPair steamData)
    {
        ID32 = steamData.ID32;
        ID64 = steamData.ID64;
    }

    public IReadOnlyCollection<AppmanifestData> Appmanifest => _appmanifest;
    public IReadOnlyCollection<AppworkshopData> Appworkshop => _appworkshop;
    public IReadOnlyCollection<UserdataData> Userdata => _userdata;
    public IReadOnlyCollection<ConfigData> Config => _config;
    public IReadOnlyCollection<LoginusersData> Loginusers => _loginusers;
    public IReadOnlyCollection<RegistryData> Registry => _registry;

    public int DetectionsCount { get; private set; }

    public SteamID32 ID32 { get; }
    public SteamID64 ID64 { get; }

    public string GetLogin()
    {
        var stringBuilder = new StringBuilder();

        foreach (var config in _config) stringBuilder.AppendLine(config.Login);

        foreach (var loginusers in _loginusers) stringBuilder.AppendLine(loginusers.Login);

        return stringBuilder.ToString();
    }

    public void Attach(ISteamIDPair account)
    {
        if (SteamIDValidator.IsSteamID64(account.ID64.AsLong) is false) return;

        switch (account)
        {
            case ConfigData configData:
                _config.Add(configData);
                break;
            case LoginusersData loginusersData:
                _loginusers.Add(loginusersData);
                break;
            case AppmanifestData appmanifestData:
                _appmanifest.Add(appmanifestData);
                break;
            case AppworkshopData appworkshopData:
                _appworkshop.Add(appworkshopData);
                break;
            case UserdataData userdataData:
                _userdata.Add(userdataData);
                break;
            case RegistryData registryData:
                _registry.Add(registryData);
                break;
            default: return;
        }

        DetectionsCount++;
    }
}