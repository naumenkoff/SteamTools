using SteamTools.Core.Utilities;

namespace SteamTools.Core.Tests.Utilities;

[TestFixture]
public class ByteConverterTests
{
    [TestCase(10, ExpectedResult = 10_485_760)]
    public long ConvertFromMegabytes_ConvertsCorrectly(long megabytes)
    {
        return ByteConverter.ConvertFromMegabytes(megabytes);
    }
}