using System.IO.Abstractions;
using AwesomeAssertions;
using Reqnroll;

namespace Smusdi.Testing.FileSystem;

[Binding]
/// <summary>
/// Reqnroll steps that operate on the (virtual) file system used during tests.
/// </summary>
public sealed class FileSystemSteps(SmusdiServiceTestingSteps steps, FileSystemTesting fileSystemTesting)
{
    private readonly IFileSystem fileSystem = steps.GetRequiredService<IFileSystem>();

    /// <summary>
    /// Given/When step: creates or replaces a file with the provided content.
    /// </summary>
    /// <param name="filePath">Relative path (virtual) of the file to write.</param>
    /// <param name="multilineText">File content.</param>
    [Given(@"the file {string} with content")]
    [When(@"I create the file {string} with content")]
    public void GivenTheFileWithContent(string filePath, string multilineText)
    {
        var realPath = fileSystemTesting.GetFullPath(filePath);

        var path = Path.GetDirectoryName(realPath);
        if (!string.IsNullOrWhiteSpace(path))
        {
            this.fileSystem.Directory.CreateDirectory(path);
        }

        this.fileSystem.File.WriteAllText(realPath, multilineText);
    }

    /// <summary>
    /// When step: deletes the specified file.
    /// </summary>
    /// <param name="filePath">Relative path (virtual) of the file to delete.</param>
    [When(@"I delete the file {string}")]
    public void WhenIDeleteTheFile(string filePath)
    {
        var realPath = fileSystemTesting.GetFullPath(filePath);
        this.fileSystem.File.Delete(realPath);
    }

    /// <summary>
    /// Then step: asserts that the specified file does not exist.
    /// </summary>
    /// <param name="filePath">Relative path (virtual) of the file to check.</param>
    [Then(@"the file {string} does not exist")]
    public void ThenTheFileDoesNotExist(string filePath)
    {
        var realPath = fileSystemTesting.GetFullPath(filePath);
        this.fileSystem.File.Exists(realPath).Should().BeFalse();
    }

    /// <summary>
    /// Then step: asserts that the specified file exists.
    /// </summary>
    /// <param name="filePath">Relative path (virtual) of the file to check.</param>
    [Then(@"the file {string} exists")]
    public void ThenTheFileExists(string filePath)
    {
        var realPath = fileSystemTesting.GetFullPath(filePath);
        this.fileSystem.File.Exists(realPath).Should().BeTrue();
    }

    /// <summary>
    /// Then step: asserts that the specified file contains the expected content.
    /// </summary>
    /// <param name="filePath">Relative path (virtual) of the file to check.</param>
    /// <param name="multilineText">Expected file content.</param>
    [Then(@"the file {string} has content")]
    public void ThenTheFileHasContent(string filePath, string multilineText)
    {
        var realPath = fileSystemTesting.GetFullPath(filePath);
        this.fileSystem.File.Exists(realPath).Should().BeTrue();
        using var stream = this.fileSystem.File.OpenRead(realPath);
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        content.Should().Be(multilineText);
    }

    /// <summary>
    /// Then step: asserts that the specified folder is empty.
    /// </summary>
    /// <param name="folderPath">Relative path (virtual) of the folder to check.</param>
    [Then(@"the folder {string} is empty")]
    public void ThenTheFolderIsEmpty(string folderPath)
    {
        var realPath = fileSystemTesting.GetFullPath(folderPath);
        this.fileSystem.Directory.Exists(realPath).Should().BeTrue();
        this.fileSystem.Directory.EnumerateFileSystemEntries(realPath).Count().Should().Be(0);
    }

    /// <summary>
    /// Then step: asserts that the specified folder is not empty.
    /// </summary>
    /// <param name="folderPath">Relative path (virtual) of the folder to check.</param>
    [Then(@"the folder {string} is not empty")]
    public void ThenTheFolderIsNotEmpty(string folderPath)
    {
        var realPath = fileSystemTesting.GetFullPath(folderPath);
        this.fileSystem.Directory.Exists(realPath).Should().BeTrue();
        this.fileSystem.Directory.EnumerateFileSystemEntries(realPath).Any().Should().BeTrue();
    }
}
