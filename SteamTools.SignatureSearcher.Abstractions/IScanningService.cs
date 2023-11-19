namespace SteamTools.SignatureSearcher.Abstractions;

public interface IScanningService
{
    ValueTask<IScanningResult> StartScanningAsync();
}