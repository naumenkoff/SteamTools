using SProject.CQRS;

namespace SteamTools.SignatureSearcher.Contracts.Responses;

public readonly struct ScanningResult : IResponse
{
    public required int ScannedFiles { get; init; }
    public required int OpenedFiles { get; init; }
    public required IReadOnlyList<string> Files { get; init; }
}