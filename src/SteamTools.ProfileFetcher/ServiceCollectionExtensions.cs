using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SteamTools.Common;
using SteamTools.ProfileFetcher.Abstractions;
using SteamTools.ProfileFetcher.Enums;
using SteamTools.ProfileFetcher.Models;
using SteamTools.ProfileFetcher.Services;

namespace SteamTools.ProfileFetcher;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProfileFetcher(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddSingleton<ICacheService, SteamApiCacheService>();

        serviceCollection.AddHttpClient<ISteamApiClient, SteamApiClient>();
        serviceCollection.AddTransient<IProfileFetcherService, ProfileFetcherService>();

        serviceCollection.AddSingleton<ITemplateProvider<SteamProfileType>, ProfileTemplateProvider>();
        serviceCollection.AddTransient<IProfileTypeResolver<SteamProfileType>, ProfileTypeResolver>();

        serviceCollection.AddOptions<SteamKeyOptions>().BindConfiguration(SteamKeyOptions.Section);

        return serviceCollection;
    }
}