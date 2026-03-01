using System.IO.Abstractions;
using Reqnroll;

namespace Smusdi.Testing.FileSystem;

[Binding]
public sealed class FileSystemTesting(SmusdiServiceTestingSteps steps, ScenarioContext scenarioContext) : IDisposable
{
    private readonly Lazy<IFileSystem> fileSystem = new(() => steps.GetRequiredService<IFileSystem>());

    /// <summary>
    /// Gets the absolute root directory used by the virtual file system for the scenario.
    /// When null, paths are used as-is.
    /// </summary>
    public string? RootDirectory { get; private set; } = null;

    /// <summary>
    /// Gets or sets a value indicating whether a random root directory should be used for the scenario.
    /// </summary>
    public bool UseRandomRootDirectory { get; set; } = false;

    /// <summary>
    /// Resolves the provided relative path to the full path within the scenario root.
    /// </summary>
    /// <param name="path">Relative path to resolve.</param>
    /// <returns>Full path to use with the filesystem APIs.</returns>
    public string GetFullPath(string path) => this.RootDirectory is not null ? Path.Combine(this.RootDirectory, path) : path;

    /// <summary>
    /// BeforeScenario hook: when the scenario is tagged to use a random root directory,
    /// this method sets up the temporary directory and schedules its creation before the
    /// tested service starts.
    /// </summary>
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

    /// <summary>
    /// Disposes the temporary root directory if it was created for the scenario.
    /// </summary>
    void IDisposable.Dispose()
    {
        if (this.UseRandomRootDirectory && this.RootDirectory is not null && this.fileSystem.IsValueCreated)
        {
            this.fileSystem.Value.Directory.Delete(this.RootDirectory, true);
        }
    }
}
