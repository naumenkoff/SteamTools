using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace SteamTools.UI.Utilities;

// ReSharper disable once InconsistentNaming
public static class DIExtensions
{
    /// <summary>
    ///     Registers all services inherited from TService as <see cref="ServiceLifetime" />> services to the specified
    ///     <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of services to register.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
    /// <param name="lifetime">The type of service <see cref="ServiceLifetime" /> to be registered.</param>
    /// <seealso cref="ServiceLifetime.Transient" />
    public static void RegisterServices<TService>(this IServiceCollection services, ServiceLifetime lifetime)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(t => typeof(TService).IsAssignableFrom(t) && !t.IsInterface);
        foreach (var type in types) services.Add(new ServiceDescriptor(typeof(TService), type, lifetime));
    }
}