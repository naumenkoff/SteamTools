using SteamTools.Core.Models;

namespace SteamTools.Core.Tests.Models;

[TestFixture]
public class SteamID64Tests
{
    [Test]
    [TestCase(76561198073887158, ExpectedResult = 113621430u)]
    public uint ToSteamID32_ShouldReturnCorrectSteamID32(long steamID64)
    {
        var steamID = new SteamID64(steamID64);
        return steamID.ToSteamID32().AsUInt;
    }

    [Test]
    [TestCase(76561198073887158, ExpectedResult = "https://steamcommunity.com/profiles/76561198073887158")]
    public string ToSteamPermanentUrl_ShouldReturnCorrectPermanentUrl(long steamID64)
    {
        var steamID = new SteamID64(steamID64);
        return steamID.ToSteamPermanentUrl();
    }
}