namespace SteamTools.Domain.Services;

public interface IFileValidator
{
    bool ContainsSteamID(string? value);
}