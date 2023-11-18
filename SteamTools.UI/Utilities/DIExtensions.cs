using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace SteamTools.UI.Utilities;

// ReSharper disable once InconsistentNaming
public static class DIExtensions
{
    /// <summary>
    ///     Registers all services inherited from TService as transient services to the specified
    ///     <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of services to register.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
    /// <seealso cref="ServiceLifetime.Transient" />
    public static void RegisterTransientServices<TService>(this IServiceCollection services)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(t => typeof(TService).IsAssignableFrom(t) && !t.IsInterface);
        foreach (var type in types) services.AddTransient(typeof(TService), type);
    }
}