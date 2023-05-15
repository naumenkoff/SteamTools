namespace SteamTools.Core.Utilities;

public static class FileSystemHelper
{
    public static DirectoryInfo GetDirectory(params string[] paths)
    {
        return GetFileSystemInfo<DirectoryInfo>(paths);
    }

    public static FileInfo GetFile(params string[] paths)
    {
        return GetFileSystemInfo<FileInfo>(paths);
    }

    private static T GetFileSystemInfo<T>(params string[] paths) where T : FileSystemInfo
    {
        if (paths is null) return null;
        if (paths.Any(string.IsNullOrEmpty)) return null;

        var path = Path.Combine(paths);

        var fileSystemInfo = typeof(T) == typeof(FileInfo)
            ? (T)(object)new FileInfo(path)
            : (T)(object)new DirectoryInfo(path);

        return FileSystemInfoExists(fileSystemInfo) ? fileSystemInfo : null;
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
        catch (Exception)
        {
            return null;
        }
    }
}