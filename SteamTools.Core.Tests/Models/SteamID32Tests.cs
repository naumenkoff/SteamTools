using SteamTools.Core.Models;

namespace SteamTools.Core.Tests.Models;

[TestFixture]
public class SteamID32Tests
{
    [Test]
    [TestCase(113621430u, ExpectedResult = "STEAM_0:0:56810715")]
    public string ToSteamID_ShouldReturnCorrectSteamID(uint steamID32)
    {
        var steamID = new SteamID32(steamID32);
        return steamID.ToSteamID();
    }

    [Test]
    [TestCase(113621430u, ExpectedResult = "[U:1:113621430]")]
    public string ToSteamID3_ShouldReturnCorrectSteamID3(uint steamID32)
    {
        var steamID = new SteamID32(steamID32);
        return steamID.ToSteamID3();
    }

    [Test]
    [TestCase(113621430u, ExpectedResult = "76561198073887158")]
    public string ToSteamID64_ShouldReturnCorrectSteamID64(uint steamID32)
    {
        var steamID = new SteamID32(steamID32);
        return steamID.ToSteamID64().AsString;
    }
}