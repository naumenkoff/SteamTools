namespace SteamTools.ProfileFetcher.Models.Responses;

// ReSharper disable ClassNeverInstantiated.Global
public class GenericSteamResponse<T>
{
    public T? Response { get; set; }
}