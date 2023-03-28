using System.Text;
using SteamTools.LocalProfileScanner.Clients;
using SteamTools.LocalProfileScanner.Models;
using SteamTools.LocalProfileScanner.Services;

namespace SteamTools.LocalProfileScanner;

public class Program
{
    public static async Task Main()
    {
        await SteamClient.InitializeAsync();
        await AccountFinderService.FindAccountsAsync();

        if (LocalAccount.Accounts.Any() is false)
        {
            Console.WriteLine("I didn't find any accounts!");
            Console.Write("The program has finished working. Press any key to exit.");
            Console.ReadKey();
            return;
        }

        const string header =
            "|--------------------| Steam ID64 > {0} [{1}] {2} < Account Login |--------------------|";
        var sb = new StringBuilder();

        foreach (ILocalAccount account in LocalAccount.Accounts)
        {
            sb.AppendLine(string.Format(header, account.Steam64, account.DetectionsCount, account.GetLogin()));

            if (account.Appmanifest?.Count() > 0)
            {
                var appmanifests = account.Appmanifest.ToList();
                for (var i = 0; i < appmanifests.Count; i++)
                {
                    var appmanifest = appmanifests[i];
                    sb.AppendLine($"appmanifest [{i + 1}/{appmanifests.Count}]:");
                    sb.AppendLine("\tPath > " + appmanifest.File.FullName);
                    sb.AppendLine("\tName > " + appmanifest.Name);
                }
            }

            if (account.Appworkshop?.Count() > 0)
            {
                var appworkshops = account.Appworkshop.ToList();
                for (var i = 0; i < appworkshops.Count; i++)
                {
                    var appworkshop = appworkshops[i];
                    sb.AppendLine($"appworkshop [{i + 1}/{appworkshops.Count}]:");
                    sb.AppendLine("\tPath > " + appworkshop.File.FullName);
                    sb.AppendLine("\tApp ID > " + appworkshop.AppID);
                }
            }

            if (account.Userdata is not null)
            {
                sb.AppendLine("Userdata:");
                sb.AppendLine("\tPath > " + account.Userdata.Directory.FullName);
            }

            if (account.Config is not null)
            {
                sb.AppendLine("Config:");
                sb.AppendLine("\tLogin > " + account.Config.Login);
            }

            if (account.Loginusers is not null)
            {
                sb.AppendLine("Loginusers:");
                sb.AppendLine("\tLogin > " + account.Loginusers.Login);
                sb.AppendLine("\tName > " + account.Loginusers.Name);
                sb.AppendLine("\tTime > " + account.Loginusers.Timestamp);
            }

            if (account.Registry is not null)
            {
                sb.AppendLine("Registry:");
                sb.AppendLine("\tKey > " + account.Registry.RegistryKey);
            }
        }

        Console.WriteLine(sb.ToString());
        Console.Write("The program has finished working. Press any key to exit.");
        Console.ReadKey();
    }
}