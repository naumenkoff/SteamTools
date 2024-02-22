using Microsoft.Extensions.DependencyInjection;
using SteamTools.Common;

namespace SteamTools.SignatureSearcher.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSignatureSearcher(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IScanningResultWriter, ScanningResult>();

        // IScanningFactory because it's Transient
        serviceCollection.AddSingleton<Func<IScanningService>>(x => x.GetRequiredService<IScanningService>);

        // IFileValidator Factory because it requires runtime value ISteamIDPair
        serviceCollection.AddSingleton<Func<ISteamIDPair, IFileValidator>>(_ => id => new FileValidator(id));

        serviceCollection.AddTransient<IScanningService, ScanningService>();
        serviceCollection.AddTransient<IFileScanner, FileScanner>();

        return serviceCollection;
    }
}