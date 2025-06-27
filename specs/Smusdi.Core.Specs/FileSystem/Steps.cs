using System.IO.Abstractions;
using AwesomeAssertions;
using Smusdi.Testing;
using Smusdi.Testing.FileSystem;

namespace Smusdi.Core.Specs.FileSystem;

[Binding]
public sealed class Steps(SmusdiServiceTestingSteps smusdiServiceTestingSteps, FileSystemTesting fileSystemTesting)
{
    private readonly IFileSystem fileSystem = smusdiServiceTestingSteps.GetRequiredService<IFileSystem>();

    [Then("the mock file system is not used")]
    public void ThenTheMockFileSystemIsNotUsed()
    {
        this.fileSystem.GetType().Should().Be(typeof(System.IO.Abstractions.FileSystem));
    }

    [Then("the root directory is set")]
    public void ThenTheRootDirectoryIsSet()
    {
        fileSystemTesting.UseRandomRootDirectory.Should().BeTrue();
    }
}
