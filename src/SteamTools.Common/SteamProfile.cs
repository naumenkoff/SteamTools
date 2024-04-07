using SProject.Steam;

namespace SteamTools.Common;

public class SteamProfile : ISteamIDPair
{
    public SteamProfile(uint id32)
    {
        ID32 = new SteamID32(id32);

        var id64 = SteamConverter.ToSteamID64(id32);
        ID64 = new SteamID64(id64);

        ID3 = SteamConverter.ToSteamID3(id32);

        ID = SteamConverter.ToSteamID(id64, out _, out _);

        Permalink = SteamConverter.ToSteamPermanentUrl(id64);
    }

    public SteamProfile(long id64)
    {
        ID64 = new SteamID64(id64);

        var id32 = SteamConverter.ToSteamID32(id64);
        ID32 = new SteamID32(id32);

        ID3 = SteamConverter.ToSteamID3(id32);

        ID = SteamConverter.ToSteamID(id64, out _, out _);

        Permalink = SteamConverter.ToSteamPermanentUrl(id64);
    }

    public SteamProfile(SteamProfile steamProfile, PlayerSummaries? playerSummaries, string request)
    {
        ID64 = steamProfile.ID64;
        ID32 = steamProfile.ID32;
        ID3 = steamProfile.ID3;
        ID = steamProfile.ID;
        Permalink = steamProfile.Permalink;

        PlayerSummaries = playerSummaries;
        Request = request;
        ExistOnline = PlayerSummaries?.SteamID == ID64.AsString;
    }

    private SteamProfile()
    {
        ExistOnline = false;
    }

    public static SteamProfile Empty { get; } = new();
    public string ID3 { get; }
    public string ID { get; }
    public string Permalink { get; }
    public PlayerSummaries? PlayerSummaries { get; }
    public string? Request { get; }
    public bool ExistOnline { get; }
    public SteamID32 ID32 { get; }
    public SteamID64 ID64 { get; }
}