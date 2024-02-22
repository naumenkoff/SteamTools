namespace SteamTools.ProfileScanner;

public interface IScanner
{
    IEnumerable<LocalResult> GetProfiles();
}