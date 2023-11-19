namespace SteamTools.ProfileScanner.Abstractions;

public interface ILocalProfileStorage
{
    IEnumerable<LocalProfile> Accounts { get; }
    Task InitializeAsync();
}