using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using SteamTools.Common;
using SteamTools.SignatureSearcher.Abstractions;
using SteamTools.SignatureSearcher.Factories;
using SteamTools.SignatureSearcher.Services;

namespace SteamTools.SignatureSearcher;

public static class ServiceCollectionExtensions
{
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public static IServiceCollection AddSignatureSearcher(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<IFactory<ISteamIDPair, FileScannerBase>, FileScannerFactory>()
            .AddSingleton<IFactory<FileProviderBase>, FileProviderFactory>()
            .AddTransient<FileProvider>()
            .AddSingleton<Func<FileProvider>>(x => x.GetRequiredService<FileProvider>)
            .AddTransient<CertainFileProvider>()
            .AddSingleton<Func<CertainFileProvider>>(x => x.GetRequiredService<CertainFileProvider>)
            .AddTransient<IScanningResultBuilder, ScanningResultBuilder>();
    }
}