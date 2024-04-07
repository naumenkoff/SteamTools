using SteamTools.Common;
using SteamTools.ProfileScanner.Enums;

namespace SteamTools.ProfileScanner.Models.ScanningResults;

public class ResultBase(ISteamIDPair steamIdPair, ResultType resultType) : ISteamIDPair
{
    public ResultType Type { get; } = resultType;
    public SteamID32 ID32 { get; } = steamIdPair.ID32;
    public SteamID64 ID64 { get; } = steamIdPair.ID64;
}