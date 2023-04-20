using SteamTools.Core.Utilities;

namespace SteamTools.Core.Tests.Utilities;

[TestFixture]
public class FileSystemHelperTests
{
    [SetUp]
    public void SetUp()
    {
        if (!Directory.Exists(TestDirectoryPath)) Directory.CreateDirectory(TestDirectoryPath);
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(TestDirectoryPath)) Directory.Delete(TestDirectoryPath, true);
    }

    private static readonly string TestDirectoryPath = Path.Combine(Path.GetTempPath(), "FileSystemHelperTests");

    [Test]
    public void GetDirectory_ReturnsDirectoryInfo_ForValidPath()
    {
        // Arrange
        var expectedPath = Path.Combine(TestDirectoryPath, "TestDirectory");
        Directory.CreateDirectory(expectedPath);

        // Act
        var directoryInfo = FileSystemHelper.GetDirectory(TestDirectoryPath, "TestDirectory");

        // Assert
        Assert.That(directoryInfo, Is.Not.Null);
        Assert.That(directoryInfo.FullName, Is.EqualTo(expectedPath));
    }

    [Test]
    public void GetDirectory_ReturnsNull_ForInvalidPath()
    {
        // Arrange

        // Act
        var directoryInfo = FileSystemHelper.GetDirectory(TestDirectoryPath, "NonexistentDirectory");

        // Assert
        Assert.That(directoryInfo, Is.Null);
    }

    [Test]
    public void GetDirectory_ReturnsNull_ForNullOrEmptyPaths()
    {
        // Arrange
        var paths = new List<string?>
        {
            default,
            null,
            string.Empty,
            " "
        };

        for (var i = 0; i < paths.Count; i++)
        for (var j = paths.Count - 1; j > i; j--)
        {
            // Act
            var directory = FileSystemHelper.GetDirectory(paths[i], paths[j]);

            // Assert
            Assert.That(directory, Is.Null);
        }
    }

    [Test]
    public void GetFile_ReturnsFileInfo_ForValidPath()
    {
        // Arrange
        var filePath = Path.Combine(TestDirectoryPath, "TestFile.txt");
        File.WriteAllText(filePath, "Test content");

        // Act
        var fileInfo = FileSystemHelper.GetFile(TestDirectoryPath, "TestFile.txt");

        // Assert
        Assert.That(fileInfo, Is.Not.Null);
        Assert.That(fileInfo.FullName, Is.EqualTo(filePath));
    }

    [Test]
    public void GetFile_ReturnsNull_ForInvalidPath()
    {
        // Arrange

        // Act
        var fileInfo = FileSystemHelper.GetFile(TestDirectoryPath, "NonexistentFile.txt");

        // Assert
        Assert.That(fileInfo, Is.Null);
    }

    [Test]
    public void GetFileOrGetDirectory_ReturnsNull_ForInvalidType()
    {
    }

    [Test]
    public void GetFile_ReturnsNull_ForNullOrEmptyPaths()
    {
        // Arrange
        var paths = new List<string?>
        {
            default,
            null,
            string.Empty,
            " "
        };

        for (var i = 0; i < paths.Count; i++)
        for (var j = paths.Count - 1; j > i; j--)
        {
            // Act
            var file = FileSystemHelper.GetFile(paths[i], paths[j]);

            // Assert
            Assert.That(file, Is.Null);
        }
    }

    [Test]
    public void ReadAllText_ReturnsFileContent_ForValidFile()
    {
        // Arrange
        var filePath = Path.Combine(TestDirectoryPath, "TestFile.txt");
        File.WriteAllText(filePath, "Test content");
        var file = new FileInfo(filePath);

        // Act
        var content = FileSystemHelper.ReadAllText(file);

        // Assert
        Assert.That(content, Is.EqualTo("Test content"));
    }

    [Test]
    public void ReadAllText_ReturnsNull_ForInvalidFile()
    {
        // Arrange
        var file = new FileInfo("NonexistentFile.txt");

        // Act
        var content = FileSystemHelper.ReadAllText(file);

        // Assert
        Assert.That(content, Is.Null);
    }

    [Test]
    public void ReadAllText_ReturnsNull_ForNullFile()
    {
        // Arrange
        var file = default(FileInfo);

        // Act
        var content = FileSystemHelper.ReadAllText(file);

        // Assert
        Assert.That(content, Is.Null);
    }

    [Test]
    public void ReadAllText_ReturnsNull_ForValidFile()
    {
        // Arrange
        var filePath = Path.Combine(TestDirectoryPath, "TestFile.txt");
        File.WriteAllText(filePath, "Test content");
        var file = new FileInfo(filePath);

        using var _ = file.Open(FileMode.Truncate);

        // Act
        var content = FileSystemHelper.ReadAllText(file);

        // Assert
        Assert.That(content, Is.Null);
    }
}