using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SteamTools.Common;

namespace SteamTools.ProfileFetcher.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProfileFetcher(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<ICacheService, SteamApiCacheService>();

        serviceCollection.AddHttpClient<ISteamApiClient, SteamApiClient>();
        serviceCollection.AddTransient<IProfileFetcherService, DefaultProfileFetcherService>();

        serviceCollection.AddSingleton<ITemplateProvider<SteamProfileType>, DefaultProfileTemplateProvider>();
        serviceCollection.AddTransient<IProfileTypeResolver<SteamProfileType>, DefaultProfileTypeResolver>();

        serviceCollection.AddOptions<SteamKeyOptions>().Bind(configuration.GetSection(SteamKeyOptions.Section));

        return serviceCollection;
    }
}