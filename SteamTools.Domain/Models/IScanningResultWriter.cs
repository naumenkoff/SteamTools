namespace SteamTools.Domain.Models;

public interface IScanningResultWriter
{
    void AddFilePath(string path);
    void MarkScannedFile();
    void MarkSuccessfullyScannedFile();
}