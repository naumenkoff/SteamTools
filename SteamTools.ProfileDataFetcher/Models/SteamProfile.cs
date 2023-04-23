using SteamTools.Core.Models;

namespace SteamTools.ProfileDataFetcher.Models;

public class SteamProfile
{
    public SteamProfile(SteamID64 steamID64, PlayerSummaries playerSummaries, string request)
    {
        SteamID64 = steamID64;
        SteamID32 = steamID64.ToSteamID32();
        IsEmpty = SteamID64 is null;
        if (IsEmpty) { return; }
        SteamID = SteamID32.ToSteamID();
        SteamID3 = SteamID32.ToSteamID3();
        ProfilePermanentUrl = steamID64.ToSteamPermanentUrl();
        PlayerSummaries = playerSummaries;
        Request = request;
    }

    private SteamProfile()
    {
        IsEmpty = true;
    }

    public string Request { get; }
    public static SteamProfile Empty { get; } = new();
    public string SteamID { get; }
    public string SteamID3 { get; }
    public SteamID32 SteamID32 { get; }
    public SteamID64 SteamID64 { get; }
    public string ProfilePermanentUrl { get; }
    public PlayerSummaries PlayerSummaries { get; }
    public bool IsEmpty { get; }
}