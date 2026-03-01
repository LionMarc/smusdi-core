using System.Collections;
using AwesomeAssertions;
using Reqnroll;
using Smusdi.Extensibility;

namespace Smusdi.Testing;

[Binding]
public sealed class EnvironmentVariablesSteps(IReqnrollOutputHelper reqnrollOutputHelper)
{
    private readonly HashSet<string> files = new HashSet<string>();

    /// <summary>
    /// Given step: sets the specified environment variable to the provided value.
    /// </summary>
    /// <param name="name">Environment variable name.</param>
    /// <param name="value">Value to set.</param>
    [Given(@"the environment variable {string} set to {string}")]
    public void GivenTheEnvironmentVariableSetTo(string name, string value) => Environment.SetEnvironmentVariable(name, value);

    /// <summary>
    /// Given step: removes all environment variables whose name starts with the given prefix.
    /// </summary>
    /// <param name="startPattern">The prefix to match environment variable names.</param>
    [Given(@"all environment variables starting with {string} removed")]
    public void GivenAllEnvironmentVariablesStartingWithRemoved(string startPattern)
    {
        var variablesToRemove = new List<string>();
        foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
        {
            if (de.Key is string key && key.StartsWith(startPattern))
            {
                variablesToRemove.Add(key);
            }
        }

        variablesToRemove.ForEach(v => Environment.SetEnvironmentVariable(v, null));
    }

    /// <summary>
    /// Given step: writes the default .env file content for the scenario.
    /// </summary>
    /// <param name="multilineText">.env file content.</param>
    [Given(@"the default \.env file")]
    public void GivenTheDefault_EnvFile(string multilineText)
    {
        this.files.Add(SmusdiConstants.DefaultEnvFile);
        File.WriteAllText(SmusdiConstants.DefaultEnvFile, multilineText);
    }

    /// <summary>
    /// Given step: writes a custom .env file with the provided content.
    /// </summary>
    /// <param name="path">Path to the .env file.</param>
    /// <param name="multilineText">File content.</param>
    [Given(@"the env file {string} with content")]
    public void GivenTheEnvFileWithContent(string path, string multilineText)
    {
        this.files.Add(path);
        File.WriteAllText(path, multilineText);
    }

    /// <summary>
    /// Then step: asserts that the given environment variable is set to the expected value.
    /// </summary>
    /// <param name="name">Variable name.</param>
    /// <param name="expectedValue">Expected value.</param>
    [Then(@"the environment variable {string} is set to {string}")]
    public void ThenTheEnvironmentVariableIsSetTo(string name, string expectedValue)
    {
        var value = Environment.GetEnvironmentVariable(name);
        value.Should().Be(expectedValue);
    }

    /// <summary>
    /// AfterScenario hook: deletes any temporary files created during the scenario.
    /// </summary>
    [AfterScenario]
    public void CleanupFiles()
    {
        foreach (var file in this.files)
        {
            reqnrollOutputHelper.WriteLine($"[EnvironmentVariablesSteps] Deleting file {file}");
            File.Delete(file);
        }
    }
}
