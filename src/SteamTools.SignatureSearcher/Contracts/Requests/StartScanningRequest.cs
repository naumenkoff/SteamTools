using SProject.CQRS;
using SteamTools.Common;
using SteamTools.SignatureSearcher.Contracts.Responses;

namespace SteamTools.SignatureSearcher.Contracts.Requests;

public class StartScanningRequest : IRequest<ScanningResult>
{
    public required CancellationToken ScanningCancellation { get; init; }
    public required ISteamIDPair SteamId { get; init; }
}