namespace SteamTools.Domain.Models;

public class SteamID32
{
    public SteamID32(uint id32)
    {
        AsUInt = id32;
        AsString = id32.ToString();
    }

    public uint AsUInt { get; }
    public string AsString { get; }
}