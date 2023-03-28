using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SteamTools.ProfileDataFetcher.Clients;
using SteamTools.ProfileDataFetcher.Providers;
using SteamTools.ProfileDataFetcher.Services;

namespace SteamTools.ProfileDataFetcher;

public class Program
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    private Program()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _serviceProvider = new ServiceCollection()
            .AddSingleton<ISteamHttpClient, SteamHttpClient>()
            .AddSingleton<ISteamProfileRegexProvider, SteamProfileRegexProvider>()
            .AddTransient<ISteamProfileTypeResolver, SteamProfileTypeResolver>()
            .AddTransient<ISteamProfileBuilder, SteamProfileBuilder>()
            .AddSingleton(_configuration)
            .AddSingleton<ISteamApiKeyProvider, SteamApiKeyProvider>()
            .BuildServiceProvider();
    }

    public static void Main()
    {
        var program = new Program();
        program.StartAsync().GetAwaiter().GetResult();
    }

    private async Task StartAsync()
    {
        await WaitForInputAsync();
    }

    private async Task WaitForInputAsync()
    {
        while (true)
        {
            var input = GetInput();
            await ProcessInputAsync(input);
        }
    }

    private async Task ProcessInputAsync(string input)
    {
        var factory = _serviceProvider.GetRequiredService<ISteamProfileBuilder>();
        var profile = await factory.BuildSteamProfileAsync(input);
        var summaries = profile.GetProfileSummaries();
        Console.WriteLine(string.IsNullOrEmpty(summaries)
            ? "Most likely, such an account doesn't exist or some error occurred.\n"
            : profile.GetProfileSummaries());
    }

    private static string GetInput()
    {
        Console.Write("Enter Steam ID64/Steam ID32/Steam ID3/Steam ID/Profile Custom Url/Profile Permanent Url > ");
        Console.ForegroundColor = ConsoleColor.Red;
        var input = Console.ReadLine();
        Console.ResetColor();
        Console.WriteLine();
        return input;
    }
}