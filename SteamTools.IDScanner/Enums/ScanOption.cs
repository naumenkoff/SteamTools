namespace SteamTools.IDScanner.Enums;

/// <summary>
///     Enum describing the scanning behavior.
/// </summary>
public enum ScanOption
{
    /// <summary>
    ///     Recursive scanning of directories and their files.
    /// </summary>
    All,

    /// <summary>
    ///     Scanning directories to find files of a given extensions
    /// </summary>
    PatternBased
}