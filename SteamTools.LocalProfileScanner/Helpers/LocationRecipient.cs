namespace SteamTools.LocalProfileScanner.Helpers;

public static class LocationRecipient
{
    public static DirectoryInfo GetDirectory(params string[] paths)
    {
        if (paths.Any(string.IsNullOrEmpty)) return null;

        var path = Path.Combine(paths);
        var directory = new DirectoryInfo(path);
        return directory;
    }

    public static FileInfo GetFile(params string[] paths)
    {
        if (paths.Any(string.IsNullOrEmpty)) return null;

        var path = Path.Combine(paths);
        var file = new FileInfo(path);
        return file;
    }

    public static bool FileSystemInfoExists(FileSystemInfo fileSystemInfo)
    {
        return fileSystemInfo is { Exists: true };
    }

    public static async Task<string> ReadFileContentAsync(FileInfo file)
    {
        if (FileSystemInfoExists(file) is false) return null;

        try
        {
            await using var stream = file.OpenRead();
            using var streamReader = new StreamReader(stream);
            var content = await streamReader.ReadToEndAsync();
            return content;
        }
        catch
        {
            return null;
        }
    }
}