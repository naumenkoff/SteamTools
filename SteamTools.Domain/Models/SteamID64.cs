namespace SteamTools.Domain.Models;

public class SteamID64
{
    public SteamID64(long id64)
    {
        AsLong = id64;
        AsString = id64.ToString();
    }

    public long AsLong { get; }
    public string AsString { get; }
}