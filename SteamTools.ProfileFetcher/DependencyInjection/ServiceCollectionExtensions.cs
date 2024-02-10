using Microsoft.Extensions.DependencyInjection;
using SteamTools.Common;

namespace SteamTools.ProfileFetcher.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProfileFetcher(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ISteamApiKeyProvider, SteamApiKeyProvider>();
        serviceCollection.AddSingleton<ICacheService, SteamApiCacheService>();
        serviceCollection.AddSingleton<ITemplateProvider<SteamProfileType>, ProfileTemplateProvider>();
        serviceCollection.AddHttpClient<ISteamApiClient, SteamApiClient>();
        serviceCollection.AddTransient<IProfileFetcherService, ProfileFetcherService>();
        serviceCollection.AddTransient<IProfileTypeResolver, ProfileTypeResolver>();
        return serviceCollection;
    }
}