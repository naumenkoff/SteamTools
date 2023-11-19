namespace SteamTools.ProfileScanner.Abstractions;

public interface IScanner
{
    IEnumerable<LocalResult> GetProfiles();
}