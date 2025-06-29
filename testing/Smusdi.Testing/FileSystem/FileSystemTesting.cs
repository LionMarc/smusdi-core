using System.IO.Abstractions;
using Reqnroll;

namespace Smusdi.Testing.FileSystem;

[Binding]
public sealed class FileSystemTesting(SmusdiServiceTestingSteps steps, ScenarioContext scenarioContext) : IDisposable
{
    private readonly Lazy<IFileSystem> fileSystem = new(() => steps.GetRequiredService<IFileSystem>());

    public string? RootDirectory { get; private set; } = null;

    public bool UseRandomRootDirectory { get; set; } = false;

    public string GetFullPath(string path) => this.RootDirectory is not null ? Path.Combine(this.RootDirectory, path) : path;

    [BeforeScenario(Order = SmusdiServiceTestingSteps.ServiceInitializationHookOrder - 1)]
    public void SetupRootDirectory()
    {
        if (scenarioContext.ScenarioInfo.CombinedTags.Contains(Constants.WithRandomFileSystemRootDirectoryTag))
        {
            this.UseRandomRootDirectory = true;
            this.RootDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            steps.BeforeStarting.Add(() => this.fileSystem.Value.Directory.CreateDirectory(this.RootDirectory));
        }
    }

    void IDisposable.Dispose()
    {
        if (this.UseRandomRootDirectory && this.RootDirectory is not null && this.fileSystem.IsValueCreated)
        {
            this.fileSystem.Value.Directory.Delete(this.RootDirectory, true);
        }
    }
}
