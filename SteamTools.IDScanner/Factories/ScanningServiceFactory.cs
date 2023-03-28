using System.Text;
using SteamTools.IDScanner.Converters;
using SteamTools.IDScanner.Services;

namespace SteamTools.IDScanner.Factories;

/// <summary>
///     Factory class for creating instances of the ScanningService.
/// </summary>
public static class ScanningServiceFactory
{
    /// <summary>
    ///     Prompts the user to enter the path to the Steam root directory and returns it as a DirectoryInfo object.
    /// </summary>
    /// <returns>The Steam root directory as a DirectoryInfo object.</returns>
    private static DirectoryInfo GetSteamDirectory()
    {
        var steamDirectoryPath = @"C:\Program Files (x86)\Steam";
        while (Directory.Exists(steamDirectoryPath) is false)
        {
            Console.Clear();
            Console.Write("Please enter a valid path to the Steam root directory: ");
            steamDirectoryPath = GetInput();
        }

        return new DirectoryInfo(steamDirectoryPath);
    }

    /// <summary>
    ///     Converts a Steam ID32 or ID64 to ID64 format.
    /// </summary>
    /// <param name="input">The Steam ID in either ID32 or ID64 format.</param>
    /// <returns>The Steam ID in ID64 format, or null if the input is invalid.</returns>
    private static string GetSteamID(string input)
    {
        var id = SteamIDConverter.IsSteamID32(input) ? SteamIDConverter.ConvertSteamID32ToSteamID64(input) : input;
        return SteamIDConverter.IsSteamID64(id) ? id : null;
    }

    /// <summary>
    ///     Prompts the user to enter a Steam ID in either ID32 or ID64 format and returns it in ID64 format.
    /// </summary>
    /// <returns>The Steam ID in ID64 format.</returns>
    private static string GetSteamID()
    {
        var id = string.Empty;
        while (string.IsNullOrEmpty(id))
        {
            Console.Clear();
            Console.Write("Please enter Steam ID64 or Steam ID32: ");
            var input = GetInput();
            id = GetSteamID(input);
        }

        return id;
    }

    /// <summary>
    ///     Prompts the user to enter the file extensions to be scanned and returns them as an array of strings.
    /// </summary>
    /// <param name="searchWithExtensions">Whether or not to search with the entered extensions.</param>
    /// <returns>The file extensions to be scanned as an array of strings.</returns>
    private static string[] GetExtensions(bool searchWithExtensions)
    {
        if (searchWithExtensions is false) return new[] { "*.vdf", "*.txt" };
        Console.Clear();
        Console.WriteLine("You need to enter file extensions for scanning.");
        Console.WriteLine(
            "Please note that the input should be in the following format: .extension, separated by space.");
        Console.WriteLine("If you don't enter anything, default extensions '*.vdf *.txt' will be applied.");
        var extensions = GetInput();
        return string.IsNullOrEmpty(extensions)
            ? new[] { "*.vdf", "*.txt" }
            : extensions.Split(' ').Where(x => x.StartsWith("*.")).Select(x => x).ToArray();
    }

    /// <summary>
    ///     Prompts the user for input and returns it as a string.
    /// </summary>
    /// <returns>The user's input as a string.</returns>
    private static string GetInput()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        var extensions = Console.ReadLine();
        Console.ResetColor();
        return extensions;
    }

    /// <summary>
    ///     Prompts the user to enter a command for setting the maximum file size for the scan.
    /// </summary>
    /// <returns>The user's command as a string.</returns>
    private static string GetMaximumFileSizeCommand()
    {
        ShowDescription("If you enter 'continue', the search will be performed for all file extensions,",
            "but limited by file size in megabytes.",
            "For example, 'continue 50' will perform the search for all file,",
            "but with a maximum file size of 50 megabytes.",
            "However, you can use the command with the -f parameter,",
            "which will take into account that it needs to search for files",
            "with the specified extension and the entered size.",
            "For example, 'continue -f 50' will search for files based on the previously entered extensions",
            "with a maximum file size of 50 megabytes.");
        var input = GetInput();
        return input;
    }

    /// <summary>
    ///     Parses a command and returns an array of string arguments based on the specified format:
    /// </summary>
    /// <param name="command">The command to parse.</param>
    /// <returns>
    ///     If the command is "continue -f", an empty array is returned.
    ///     If the command is "continue maximumFileSize", an array with the maximum file size is returned.
    ///     If the command is "continue -f maximumFileSize", an array with the maximum file size and the "-f" argument is
    ///     returned.
    ///     If the command is "continue" and the flag is not "-f" but the maximum file size is specified, for example,
    ///     "continue -s 50", an array with the maximum file size is returned.
    ///     Otherwise, an empty array is returned.
    /// </returns>
    public static string[] ParseCommand(string command)
    {
        if (string.IsNullOrEmpty(command) || !command.StartsWith("continue")) return Array.Empty<string>();
        var parts = command.Split();
        return parts.Length switch
        {
            2 when parts[1].All(char.IsDigit) is false => Array.Empty<string>(),
            3 when parts[1] != "-f" => new[] { parts[2] },
            3 when parts[1] == "-f" => new[] { parts[2], "-f" },
            2 when parts[1].All(char.IsDigit) => new[] { parts[1] },
            _ => Array.Empty<string>()
        };
    }

    /// <summary>
    ///     Clears the console and displays the provided messages on separate lines.
    /// </summary>
    /// <param name="messages">The messages to display.</param>
    private static void ShowDescription(params string[] messages)
    {
        if (messages is null || messages.Length == 0) return;
        var sb = new StringBuilder();
        foreach (var message in messages) sb.AppendLine(message);

        Console.Clear();
        Console.WriteLine(sb.ToString());
    }

    /// <summary>
    ///     Determines if the search should include file extensions based on the provided command-line arguments.
    /// </summary>
    /// <param name="fileSizeCommand">The collection of command-line arguments.</param>
    /// <returns>True if the "-f" option is present in the command-line arguments, false otherwise.</returns>
    private static bool SearchWithExtensions(IEnumerable<string> fileSizeCommand)
    {
        return fileSizeCommand.Contains("-f");
    }

    /// <summary>
    ///     Returns the maximum file size specified in the command line arguments.
    ///     If not specified, the default maximum size of 100 MB is returned.
    /// </summary>
    /// <param name="fileSizeCommand">A read-only list of command line arguments that may contain the maximum file size option.</param>
    /// <returns>The maximum file size in megabytes.</returns>
    private static int GetMaximumFileSize(IReadOnlyList<string> fileSizeCommand)
    {
        return fileSizeCommand.Count > 0 ? int.Parse(fileSizeCommand[0]) : 100;
    }

    /// <summary>
    ///     Factory method for creating a new ScanningService instance.
    /// </summary>
    /// <returns>
    ///     Returns a new ScanningService instance with settings specified by the user, such as
    ///     Steam directory, Steam ID, file extensions, and maximum file size.
    /// </returns>
    public static ScanningService GetScanningService()
    {
        // 76561198073887158
        var steamDirectory = GetSteamDirectory();
        var steamID = GetSteamID();
        var fileSizeCommand = ParseCommand(GetMaximumFileSizeCommand());
        var searchWithExtensions = SearchWithExtensions(fileSizeCommand);
        var maximumFileSize = GetMaximumFileSize(fileSizeCommand);
        var extensions = GetExtensions(searchWithExtensions);
        Console.Clear();
        return searchWithExtensions
            ? new ScanningService(steamDirectory, steamID, maximumFileSize, extensions)
            : new ScanningService(steamDirectory, steamID, maximumFileSize);
    }
}