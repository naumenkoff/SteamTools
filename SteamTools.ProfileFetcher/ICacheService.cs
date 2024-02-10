namespace SteamTools.ProfileFetcher;

/// <summary>
///     A service for caching Steam API objects.
/// </summary>
public interface ICacheService
{
    /// <summary>
    ///     Adds a response object to the cache dictionary.
    /// </summary>
    /// <typeparam name="T1">The type of the cache key.</typeparam>
    /// <typeparam name="T2">The type of the response object.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="response">The response object.</param>
    void Cache<T1, T2>(T1 key, T2 response);

    /// <summary>
    ///     Gets a cached object from the cache dictionary.
    /// </summary>
    /// <typeparam name="T1">The type of the cached object.</typeparam>
    /// <typeparam name="T2">The type of the cache key.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <returns>The cached object, or the default value of <typeparamref name="T1" /> if the key is not found.</returns>
    T1? GetFromCache<T1, T2>(T2 key);
}