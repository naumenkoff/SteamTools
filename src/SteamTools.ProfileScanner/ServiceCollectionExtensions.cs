using Microsoft.Extensions.DependencyInjection;
using SteamTools.ProfileScanner.Abstractions;
using SteamTools.ProfileScanner.Services;

namespace SteamTools.ProfileScanner;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProfileScanner(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IProfileScannerService, ProfileScannerService>();
        serviceCollection.RegisterServices<IScanner>(ServiceLifetime.Singleton);
        return serviceCollection;
    }

    private static void RegisterServices<TService>(this IServiceCollection services, ServiceLifetime lifetime)
    {
        foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.IsClass))
            if (typeof(TService).IsAssignableFrom(type))
                services.Add(new ServiceDescriptor(typeof(TService), type, lifetime));
    }
}