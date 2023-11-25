namespace SteamTools.SignatureSearcher.Abstractions;

public interface IScanningService
{
    Task<IScanningResult> StartScanningAsync();
}