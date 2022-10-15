using System.IO.Abstractions;
using TechTalk.SpecFlow;

namespace Smusdi.Testing.FileSystem;

[Binding]
public sealed class FileSystemSteps
{
    private readonly IFileSystem? fileSystem;

    public FileSystemSteps(SmusdiTestingService smusdiTestingService) => this.fileSystem = smusdiTestingService.GetService<IFileSystem>();

    [Given(@"the file ""(.*)"" with content")]
    public void GivenTheFileWithContent(string p0, string multilineText)
    {
        this.fileSystem?.Directory.CreateDirectory(Path.GetDirectoryName(p0));
        this.fileSystem?.File.WriteAllText(p0, multilineText);
    }
}
