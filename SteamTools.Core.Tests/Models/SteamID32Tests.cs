using SteamTools.Core.Models;

namespace SteamTools.Core.Tests.Models;

[TestFixture]
public class SteamID32Tests
{
    [Test]
    [TestCase(113621430u, ExpectedResult = "113621430")]
    public string ToString_ShouldReturnID32AsString(uint steamID32)
    {
        var steamID = new SteamID32(steamID32);
        return steamID.ToString();
    }

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
        return steamID.ToSteamID64();
    }

    [Test]
    [TestCase(113621430u, ExpectedResult = "113621430")]
    public string ImplicitConversionOperator_ShouldReturnID32AsString(uint id32)
    {
        var steamID32 = new SteamID32(id32);
        return steamID32;
    }

    [Test]
    [TestCase(76561198073887158, ExpectedResult = 113621430u)]
    public uint Constructor_ShouldConvertSteamID64ToSteamID32(long id64)
    {
        var steamID64 = new SteamID64(id64);
        var steamID32 = new SteamID32(steamID64);
        return steamID32.AsUInt;
    }
}