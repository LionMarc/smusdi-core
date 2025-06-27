using System.IO.Abstractions;
using AwesomeAssertions;
using Reqnroll;

namespace Smusdi.Testing.FileSystem;

[Binding]
public sealed class FileSystemSteps(SmusdiServiceTestingSteps steps, FileSystemTesting fileSystemTesting)
{
    private readonly IFileSystem fileSystem = steps.GetRequiredService<IFileSystem>();

    [Given(@"the file ""(.*)"" with content")]
    [When(@"I create the file ""(.*)"" with content")]
    public void GivenTheFileWithContent(string p0, string multilineText)
    {
        var realPath = fileSystemTesting.GetFullPath(p0);

        var path = Path.GetDirectoryName(realPath);
        if (!string.IsNullOrWhiteSpace(path))
        {
            this.fileSystem.Directory.CreateDirectory(path);
        }

        this.fileSystem.File.WriteAllText(realPath, multilineText);
    }

    [When(@"I delete the file ""(.*)""")]
    public void WhenIDeleteTheFile(string p0)
    {
        var realPath = fileSystemTesting.GetFullPath(p0);
        this.fileSystem.File.Delete(realPath);
    }

    [Then(@"the file ""(.*)"" does not exist")]
    public void ThenTheFileDoesNotExist(string p0)
    {
        var realPath = fileSystemTesting.GetFullPath(p0);
        this.fileSystem.File.Exists(realPath).Should().BeFalse();
    }

    [Then(@"the file ""(.*)"" exists")]
    public void ThenTheFileExists(string p0)
    {
        var realPath = fileSystemTesting.GetFullPath(p0);
        this.fileSystem.File.Exists(realPath).Should().BeTrue();
    }

    [Then(@"the file ""(.*)"" has content")]
    public void ThenTheFileHasContent(string p0, string multilineText)
    {
        var realPath = fileSystemTesting.GetFullPath(p0);
        this.fileSystem.File.Exists(realPath).Should().BeTrue();
        using var stream = this.fileSystem.File.OpenRead(realPath);
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        content.Should().Be(multilineText);
    }

    [Then(@"the folder ""(.*)"" is empty")]
    public void ThenTheFolderIsEmpty(string folder)
    {
        var realPath = fileSystemTesting.GetFullPath(folder);
        this.fileSystem.Directory.Exists(realPath).Should().BeTrue();
        this.fileSystem.Directory.EnumerateFileSystemEntries(realPath).Count().Should().Be(0);
    }

    [Then(@"the folder ""(.*)"" is not empty")]
    public void ThenTheFolderIsNotEmpty(string folder)
    {
        var realPath = fileSystemTesting.GetFullPath(folder);
        this.fileSystem.Directory.Exists(realPath).Should().BeTrue();
        this.fileSystem.Directory.EnumerateFileSystemEntries(realPath).Any().Should().BeTrue();
    }
}
