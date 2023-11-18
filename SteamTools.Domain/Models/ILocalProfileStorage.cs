namespace SteamTools.Domain.Models;

public interface ILocalProfileStorage
{
    IEnumerable<LocalProfile> Accounts { get; }
    Task InitializeAsync();
}