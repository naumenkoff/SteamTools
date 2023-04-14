namespace SteamTools.Core.Utilities;

public static class FileSystemHelper
{
    public static DirectoryInfo GetDirectory(params string[] paths)
    {
        if (paths.Any(string.IsNullOrEmpty)) return default;

        var path = Path.Combine(paths);
        var directory = new DirectoryInfo(path);

        return FileSystemInfoExists(directory) ? directory : null;
    }

    public static FileInfo GetFile(params string[] paths)
    {
        if (paths.Any(string.IsNullOrEmpty)) return default;

        var path = Path.Combine(paths);
        var file = new FileInfo(path);

        return FileSystemInfoExists(file) ? file : null;
    }

    private static bool FileSystemInfoExists(FileSystemInfo fileSystemInfo)
    {
        return fileSystemInfo is not null && fileSystemInfo.Exists;
    }

    public static string ReadAllText(FileInfo file)
    {
        if (FileSystemInfoExists(file) is false) return null;

        try
        {
            using var streamReader = file.OpenText();
            var content = streamReader.ReadToEnd();
            return content;
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"FileNotFoundException: {ex.Message}");
            return default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to read file: {ex.Message}");
            return default;
        }
    }
}