namespace SteamTools.SignatureSearcher.Abstractions;

public interface IScanningResultWriter
{
    void AddFilePath(string path);
    void MarkScannedFile();
    void MarkSuccessfullyScannedFile();
}