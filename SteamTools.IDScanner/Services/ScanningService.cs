using SteamTools.IDScanner.Converters;
using SteamTools.IDScanner.Enums;

namespace SteamTools.IDScanner.Services;

/// <summary>
///     Provides the functionality for scanning files and directories for a given Steam ID in their contents.
/// </summary>
public class ScanningService
{
    private readonly string[] _extensions;
    private readonly long _maximumFileSize;
    private readonly DirectoryInfo _rootDirectory;
    private readonly ScanOption _scanOption;
    private readonly string _steam32ID;
    private readonly string _steam64ID;
    private int _numberOfScannedDirectories;
    private int _numberOfScannedFiles;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ScanningService" /> class for scanning all files in the specified root
    ///     directory and its subdirectories.
    /// </summary>
    /// <param name="rootDirectory">The root directory to start scanning.</param>
    /// <param name="steamID64">The Steam ID64 to search for in file contents.</param>
    /// <param name="maximumFileSize">The maximum size of files to scan, in megabytes. Set to 0 to disable the limit.</param>
    public ScanningService(DirectoryInfo rootDirectory, string steamID64, int maximumFileSize)
    {
        Console.Clear();
        Console.WriteLine(
            $"Steam Directory Path > '{rootDirectory.FullName}'.\nSteam ID64 > '{steamID64}'.\nMaximum File Size > '{maximumFileSize}' MB.\n");

        _rootDirectory = rootDirectory;
        _scanOption = ScanOption.All;

        _steam64ID = steamID64;
        _steam32ID = SteamIDConverter.ConvertSteamID64ToSteamID32(steamID64);

        _maximumFileSize = ByteConverter.ConvertFromMegabytes(maximumFileSize);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ScanningService" /> class for scanning files with the specified
    ///     extensions in the specified root directory and its subdirectories.
    /// </summary>
    /// <param name="rootDirectory">The root directory to start scanning.</param>
    /// <param name="steamID64">The Steam ID64 to search for in file contents.</param>
    /// <param name="maximumFileSize">The maximum size of files to scan, in megabytes. Set to 0 to disable the limit.</param>
    /// <param name="extensions">The file extensions to scan for, e.g. "*.txt", "*.vdf".</param>
    public ScanningService(DirectoryInfo rootDirectory, string steamID64, int maximumFileSize,
        params string[] extensions)
    {
        Console.Clear();
        Console.WriteLine(
            $"Extensions > '{string.Join(", ", extensions)}'.\nSteam Directory Path > '{rootDirectory.FullName}'.\nSteam ID64 > '{steamID64}'.\nMaximum File Size > '{maximumFileSize}' MB.\n");

        _rootDirectory = rootDirectory;
        _maximumFileSize = ByteConverter.ConvertFromMegabytes(maximumFileSize);

        _steam64ID = steamID64;
        _steam32ID = SteamIDConverter.ConvertSteamID64ToSteamID32(steamID64);

        _scanOption = ScanOption.PatternBased;
        _extensions = extensions;
    }

    /// <summary>
    ///     Starts the scanning process for the specified files and directories.
    /// </summary>
    public void StartScanning()
    {
        if (_scanOption == ScanOption.PatternBased) PatternBasedDirectoryScanning();
        else RecursiveDirectoryScanning(_rootDirectory);
    }

    /// <summary>
    ///     Recursively scans directories for files containing the Steam ID.
    /// </summary>
    /// <param name="directory">The root directory to scan for files.</param>
    private void RecursiveDirectoryScanning(DirectoryInfo directory)
    {
        var detectedDirectories = directory.GetDirectories();
        var directoryFiles = directory.GetFiles();

        Parallel.Invoke(() => Parallel.ForEach(detectedDirectories, RecursiveDirectoryScanning),
            () => Parallel.ForEach(directoryFiles, ScanFile));

        _numberOfScannedDirectories++;
    }

    /// <summary>
    ///     Scans files for the given extensions in the specified directory and its subdirectories.
    /// </summary>
    private void PatternBasedDirectoryScanning()
    {
        Parallel.ForEach(_extensions, extension =>
        {
            var files = _rootDirectory.GetFiles(extension, SearchOption.AllDirectories);
            Parallel.ForEach(files, ScanFile);
        });
    }

    /// <summary>
    ///     Scans the given file for the Steam ID.
    /// </summary>
    /// <param name="file">The file to scan.</param>
    private void ScanFile(FileInfo file)
    {
        _numberOfScannedFiles++;
        Console.Title = $"Scanned ['D']: {_numberOfScannedDirectories} ['F']: {_numberOfScannedFiles}";
        if (_maximumFileSize != 0 && file.Length > _maximumFileSize) return;
        var fileContent = GetFileContent(file);
        if (fileContent is null || IsSteamIDPresentHere(fileContent) is false) return;
        Console.WriteLine($"Account found in file \"{file}\".");
    }

    /// <summary>
    ///     Gets the content of the given file as a string.
    /// </summary>
    /// <param name="file">The file to read.</param>
    /// <returns>The content of the file as a string, or null if an error occurs while reading the file.</returns>
    private static string GetFileContent(FileInfo file)
    {
        try
        {
            using var stream = file.OpenRead();
            using var streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }
        catch (Exception) // skipcq: CS-R1008
        {
            return null;
        }
    }

    /// <summary>
    ///     Checks if the specified Steam ID is present in the given text.
    /// </summary>
    /// <param name="text">The text to search for the Steam ID.</param>
    /// <returns>true if the Steam ID is found, otherwise false.</returns>
    private bool IsSteamIDPresentHere(string text)
    {
        return text.Contains(_steam32ID) || text.Contains(_steam64ID);
    }
}