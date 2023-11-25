namespace SteamTools.SignatureSearcher.Abstractions;

public interface IFileValidator
{
    bool ContainsSteamId(string? value);
}