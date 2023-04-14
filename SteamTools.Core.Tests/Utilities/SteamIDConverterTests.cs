using SteamTools.Core.Utilities;

namespace SteamTools.Core.Tests.Utilities;

[TestFixture]
public class SteamIDConverterTests
{
    [TestCase(113621430u, ExpectedResult = 76561198073887158L)]
    [TestCase(95881860u, ExpectedResult = 76561198056147588L)]
    public long ToSteamID64_Converts32To64Bit(uint steamID32)
    {
        return SteamIDConverter.ToSteamID64(steamID32);
    }

    [TestCase(76561198073887158L, ExpectedResult = 113621430u)]
    [TestCase(76561198056147588L, ExpectedResult = 95881860u)]
    public uint ToSteamID32_Converts64To32Bit(long steamID64)
    {
        return SteamIDConverter.ToSteamID32(steamID64);
    }

    [TestCase(76561198073887158L, ExpectedResult = "https://steamcommunity.com/profiles/76561198073887158")]
    [TestCase(76561198056147588L, ExpectedResult = "https://steamcommunity.com/profiles/76561198056147588")]
    public string ToSteamPermanentUrl_Converts64BitToUrl(long steamID64)
    {
        return SteamIDConverter.ToSteamPermanentUrl(steamID64);
    }

    [TestCase(113621430u, ExpectedResult = "[U:1:113621430]")]
    [TestCase(95881860u, ExpectedResult = "[U:1:95881860]")]
    public string ToSteamID3_Converts32BitToSteamID3(uint steamID32)
    {
        return SteamIDConverter.ToSteamID3(steamID32);
    }

    [TestCase(113621430u, ExpectedResult = "STEAM_0:0:56810715")]
    [TestCase(95881860u, ExpectedResult = "STEAM_0:0:47940930")]
    public string ToSteamID_Converts32BitToSteamID(uint steamID32)
    {
        return SteamIDConverter.ToSteamID(steamID32);
    }

    [TestCase(0, 56810715, ExpectedResult = 76561198073887158L)]
    [TestCase(0, 47940930, ExpectedResult = 76561198056147588L)]
    public long ToSteamID64_ConvertsTo64BitSteamID(byte type, long accountNumber)
    {
        return SteamIDConverter.ToSteamID64(type, accountNumber);
    }
}