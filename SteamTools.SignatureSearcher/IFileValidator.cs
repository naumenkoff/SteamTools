namespace SteamTools.SignatureSearcher;

public interface IFileValidator
{
    bool ContainsSteamId(string? value);
}