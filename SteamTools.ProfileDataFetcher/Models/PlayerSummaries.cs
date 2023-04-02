namespace SteamTools.ProfileDataFetcher.Models;

public class PlayerSummaries
{
    private const string DefaultAvatar =
        "https://avatars.cloudflare.steamstatic.com/fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb_full.jpg";

    public string SteamID { get; set; }
    public int CommunityVisibilityState { get; set; }
    public int ProfileState { get; set; }
    public string PersonaName { get; set; }
    public string ProfileUrl { get; set; }
    public string Avatar { get; set; } = DefaultAvatar;
    public string AvatarMedium { get; set; }
    public string AvatarFull { get; set; }
    public string AvatarHash { get; set; }
    public int PersonaState { get; set; }
    public string RealName { get; set; }
    public string PrimaryClanID { get; set; }
    public int TimeCreated { get; set; }
    public int PersonaStateFlags { get; set; }
    public string LocCountryCode { get; set; }
    public string LocStateCode { get; set; }
    public int LocCityID { get; set; }
}