using System.IO.Abstractions;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Smusdi.Testing.FileSystem;

[Binding]
public sealed class FileSystemSteps
{
    private readonly IFileSystem fileSystem;

    public FileSystemSteps(SmusdiTestingService smusdiTestingService) => this.fileSystem = smusdiTestingService.GetService<IFileSystem>() ?? throw new NullReferenceException("IFileSystem");

    [Given(@"the file ""(.*)"" with content")]
    [When(@"I create the file ""(.*)"" with content")]
    public void GivenTheFileWithContent(string p0, string multilineText)
    {
        var path = Path.GetDirectoryName(p0);
        if (!string.IsNullOrWhiteSpace(path))
        {
            this.fileSystem.Directory.CreateDirectory(path);
        }

        this.fileSystem.File.WriteAllText(p0, multilineText);
    }

    [When(@"I delete the file ""(.*)""")]
    public void WhenIDeleteTheFile(string p0)
    {
        this.fileSystem.File.Delete(p0);
    }

    [Then(@"the file ""(.*)"" does not exist")]
    public void ThenTheFileDoesNotExist(string p0)
    {
        this.fileSystem.File.Exists(p0).Should().BeFalse();
    }

    [Then(@"the file ""(.*)"" exists")]
    public void ThenTheFileExists(string p0)
    {
        this.fileSystem.File.Exists(p0).Should().BeTrue();
    }

    [Then(@"the file ""(.*)"" has content")]
    public void ThenTheFileHasContent(string p0, string multilineText)
    {
        this.fileSystem.File.Exists(p0).Should().BeTrue();
        using var stream = this.fileSystem.File.OpenRead(p0);
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        content.Should().Be(multilineText);
    }
}
