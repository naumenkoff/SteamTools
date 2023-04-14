using SteamTools.Core.Utilities;

namespace SteamTools.Core.Tests.Utilities;

[TestFixture]
public class SteamIDValidatorTests
{
    [TestCase(null, ExpectedResult = false)]
    [TestCase("", ExpectedResult = false)]
    [TestCase(" ", ExpectedResult = false)]
    public bool IsSteamID64_ReturnsFalse_ForNullOrEmptyInput(string input)
    {
        return SteamIDValidator.IsSteamID64(input);
    }

    [TestCase("76561198073887158", ExpectedResult = true)]
    [TestCase("76561198056147588", ExpectedResult = true)]
    public bool IsSteamID64_ReturnsTrue_ForValidSteamID64(string steamID64)
    {
        return SteamIDValidator.IsSteamID64(steamID64);
    }

    [TestCase("76561198O73887158", ExpectedResult = false)]
    [TestCase("7656119805614758", ExpectedResult = false)]
    [TestCase("76560198056147588", ExpectedResult = false)]
    public bool IsSteamID64_ReturnsFalse_ForInvalidSteamID64(string steamID64)
    {
        return SteamIDValidator.IsSteamID64(steamID64);
    }

    [TestCase(76561198073887158, ExpectedResult = true)]
    [TestCase(76561198056147588, ExpectedResult = true)]
    public bool IsSteamID64_ReturnsTrue_ForValidSteamID64AsLong(long steamID64)
    {
        return SteamIDValidator.IsSteamID64(steamID64);
    }

    [TestCase(7656119807388715, ExpectedResult = false)]
    [TestCase(765611980561475888, ExpectedResult = false)]
    public bool IsSteamID64_ReturnsFalse_ForInvalidSteamID64AsLong(long steamID64)
    {
        return SteamIDValidator.IsSteamID64(steamID64);
    }

    [TestCase(null, ExpectedResult = false)]
    [TestCase("", ExpectedResult = false)]
    [TestCase(" ", ExpectedResult = false)]
    public bool IsSteamID32_ReturnsFalse_ForNullOrEmptyInput(string input)
    {
        return SteamIDValidator.IsSteamID32(input);
    }

    [TestCase("113621430", ExpectedResult = true)]
    [TestCase("95881860", ExpectedResult = true)]
    public bool IsSteamID32_ReturnsTrue_ForValidSteamID32(string steamID32)
    {
        return SteamIDValidator.IsSteamID32(steamID32);
    }

    [TestCase("11362143O", ExpectedResult = false)]
    [TestCase("95881860000", ExpectedResult = false)]
    public bool IsSteamID32_ReturnsFalse_ForInvalidSteamID32(string steamID32)
    {
        return SteamIDValidator.IsSteamID32(steamID32);
    }

    [TestCase(113621430u, ExpectedResult = true)]
    public bool IsSteamID32_ReturnsTrue_ForValidSteamID32AsUInt(uint steamID32)
    {
        return SteamIDValidator.IsSteamID32(steamID32);
    }

    [TestCase(uint.MinValue, ExpectedResult = false)]
    public bool IsSteamID32_ReturnsFalse_ForInvalidSteamID32AsUInt(uint steamID32)
    {
        return SteamIDValidator.IsSteamID32(steamID32);
    }
}