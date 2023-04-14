using System.Collections.Concurrent;
using SteamTools.Core.Models;

namespace SteamTools.IDScanner.Services;

public class ScanningService : IScanningService
{
    // SRP, => 
    // нужен интерфейс DirectoryScanner и FileScanner
    // 3 реализации = Extension, All и File сканнеры
    // вынести IsSteamIDPresentHere в отдельный класс валидатор
    // разделить метод StartScanning
    // обработать исключения

    private readonly ISteamClient _steamClient;
    private string[] _extensions;
    private bool _hasExtensions;
    private long _maximumFileSize;
    private bool _maximumFileSizeLimited;
    private ConcurrentBag<string> _paths;
    private SteamID32 _steam32ID;
    private SteamID64 _steam64ID;

    public ScanningService(ISteamClient steamClient)
    {
        _steamClient = steamClient;
    }

    public int TotalFileCount { get; private set; }
    public int ScannedFileCount { get; private set; }

    public async Task<List<string>> StartScanning(SteamID64 steamID64, bool maximumFileSizeLimit, long maximumFileSize,
        bool hasExtensions, CancellationToken cancellationToken, params string[] extensions)
    {
        _paths = new ConcurrentBag<string>();
        TotalFileCount = 0;
        ScannedFileCount = 0;

        _maximumFileSizeLimited = maximumFileSizeLimit;
        if (_maximumFileSizeLimited) _maximumFileSize = maximumFileSize;

        _steam64ID = steamID64;
        _steam32ID = steamID64.ToSteamID32();

        _hasExtensions = hasExtensions;
        if (_hasExtensions) _extensions = extensions;

        var options = new ParallelOptions
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = (int)Math.Max(Environment.ProcessorCount / 2.0, 1)
        };
        await Parallel.ForEachAsync(_steamClient.SteamLibraries, options, async (steamLibrary, _) =>
        {
            if (_hasExtensions) await ScanFilesOnlyWithSpecifiedExtensions(steamLibrary, options);
            else await ScanAllFiles(steamLibrary, options);
        });
        return _paths.OrderBy(x => x.Length).ToList();
    }

    private Task ScanAllFiles(DirectoryInfo directory, ParallelOptions options)
    {
        var token = options.CancellationToken;
        var files = directory.GetFiles("*.*", SearchOption.AllDirectories);
        Parallel.ForEach(files, options, file =>
        {
            if (token.IsCancellationRequested) return;
            token.ThrowIfCancellationRequested();

            ScanFile(file, token);
        });

        return Task.CompletedTask;
    }

    private Task ScanFilesOnlyWithSpecifiedExtensions(DirectoryInfo directory, ParallelOptions options)
    {
        var token = options.CancellationToken;
        Parallel.ForEach(_extensions, options, extension =>
        {
            var files = directory.GetFiles(extension, SearchOption.AllDirectories);
            Parallel.ForEach(files, options, file =>
            {
                if (token.IsCancellationRequested) return;
                token.ThrowIfCancellationRequested();

                ScanFile(file, token);
            });
        });

        return Task.CompletedTask;
    }

    private void ScanFile(FileInfo file, CancellationToken token)
    {
        TotalFileCount++;
        if (file is null) return;
        if (_maximumFileSizeLimited && file.Length > _maximumFileSize) return;
        try
        {
            using var streamReader = file.OpenText();
            ScannedFileCount++;

            if (token.IsCancellationRequested) return;
            token.ThrowIfCancellationRequested();

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (IsSteamIDPresentHere(line) is false) continue;
                _paths.Add(file.FullName);
                return;
            }
        }
        catch
        {
            // ignore
        }
    }

    private bool IsSteamIDPresentHere(string text)
    {
        return text.Contains(_steam32ID) || text.Contains(_steam64ID);
    }
}