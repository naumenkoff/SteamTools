using Microsoft.Extensions.DependencyInjection;

namespace SteamTools.ProfileScanner.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProfileScanner(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IProfileScannerService, ProfileScannerService>();
        serviceCollection.RegisterServices<IScanner>(ServiceLifetime.Transient);
        return serviceCollection;
    }

    private static void RegisterServices<TService>(this IServiceCollection services, ServiceLifetime lifetime)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(t => typeof(TService).IsAssignableFrom(t) && !t.IsInterface);
        foreach (var type in types) services.Add(new ServiceDescriptor(typeof(TService), type, lifetime));
    }
}