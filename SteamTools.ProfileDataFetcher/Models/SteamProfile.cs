namespace SteamTools.ProfileDataFetcher.Models;

public class SteamProfile
{
    public SteamProfile(SteamID64 steamID64, PlayerSummaries playerSummaries)
    {
        SteamID64 = steamID64;
        SteamID32 = steamID64.ToSteamID32();
        SteamID = SteamID32.ToSteamID();
        SteamID3 = SteamID32.ToSteamID3();
        ProfilePermanentUrl = steamID64.ToSteamPermanentUrl();
        PlayerSummaries = playerSummaries;
    }

    public SteamProfile(SteamID64 steamID64, SteamID32 steamID32, PlayerSummaries playerSummaries)
    {
        SteamID64 = steamID64;
        SteamID32 = steamID32;
        SteamID = steamID32.ToSteamID();
        SteamID3 = steamID32.ToSteamID3();
        ProfilePermanentUrl = SteamID64.ToSteamPermanentUrl();
        PlayerSummaries = playerSummaries;
    }

    private SteamProfile()
    {
    }

    public static SteamProfile Empty => new();
    public string SteamID { get; }
    public string SteamID3 { get; }
    public SteamID32 SteamID32 { get; }
    public SteamID64 SteamID64 { get; }
    public string ProfilePermanentUrl { get; }
    public PlayerSummaries PlayerSummaries { get; }
}