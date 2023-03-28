namespace SteamTools.ProfileDataFetcher.Models;

public class GenericSteamResponse<T>
{
    public T Response { get; set; }
}