namespace SteamTools.SignatureSearcher;

public interface IScanningResultWriter
{
    void AddFilePath(string path);
    void MarkScannedFile();
    void MarkSuccessfullyScannedFile();
}