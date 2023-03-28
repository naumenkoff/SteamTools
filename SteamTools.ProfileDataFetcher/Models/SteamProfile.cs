using System.Text;

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
        ProfileCustomUrl = playerSummaries.ProfileUrl;
        PlayerSummaries = playerSummaries;
    }

    public SteamProfile(SteamID64 steamID64, SteamID32 steamID32, PlayerSummaries playerSummaries)
    {
        SteamID64 = steamID64;
        SteamID32 = steamID32;
        SteamID = steamID32.ToSteamID();
        SteamID3 = steamID32.ToSteamID3();
        ProfilePermanentUrl = SteamID64.ToSteamPermanentUrl();
        ProfileCustomUrl = playerSummaries.ProfileUrl;
        PlayerSummaries = playerSummaries;
    }

    private SteamProfile()
    {
    }

    public static SteamProfile Empty => new();
    public string ProfileCustomUrl { get; }
    public string SteamID { get; }
    public string SteamID3 { get; }
    public SteamID32 SteamID32 { get; }
    public SteamID64 SteamID64 { get; }
    public string ProfilePermanentUrl { get; }
    public PlayerSummaries PlayerSummaries { get; }

    public string GetProfileSummaries()
    {
        if (SteamID64 is null) return null;

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Steam ID > " + SteamID);
        stringBuilder.AppendLine("Steam ID3 > " + SteamID3);
        stringBuilder.AppendLine("Steam ID32 > " + SteamID32);
        stringBuilder.AppendLine("Steam ID64 > " + SteamID64);
        stringBuilder.AppendLine("Profile Custom Url > " + ProfileCustomUrl);
        stringBuilder.AppendLine("Profile Permanent Url > " + ProfilePermanentUrl);

        if (PlayerSummaries is null) return stringBuilder.ToString();

        stringBuilder.AppendLine($"Profile Name > {PlayerSummaries.PersonaName}");
        stringBuilder.AppendLine($"Avatar > {PlayerSummaries.AvatarFull}");

        return stringBuilder.ToString();
    }
}