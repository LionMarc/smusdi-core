using System.Collections;
using FluentAssertions;
using Smusdi.Core;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace Smusdi.Testing;

[Binding]
public sealed class EnvironmentVariablesSteps
{
    private readonly HashSet<string> files = new HashSet<string>();
    private readonly ISpecFlowOutputHelper specFlowOutputHelper;

    public EnvironmentVariablesSteps(ISpecFlowOutputHelper specFlowOutputHelper) => this.specFlowOutputHelper = specFlowOutputHelper;

    [Given(@"the environment variable ""(.*)"" set to ""(.*)""")]
    public void GivenTheEnvironmentVariableSetTo(string p0, string p1)
    {
        Environment.SetEnvironmentVariable(p0, p1);
    }

    [Given(@"all environment variables starting with ""(.*)"" removed")]
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

    [Given(@"the default \.env file")]
    public void GivenTheDefault_EnvFile(string multilineText)
    {
        this.files.Add(SmusdiConstants.DefaultEnvFile);
        File.WriteAllText(SmusdiConstants.DefaultEnvFile, multilineText);
    }

    [Given(@"the env file ""(.*)"" with content")]
    public void GivenTheEnvFileWithContent(string path, string multilineText)
    {
        this.files.Add(path);
        File.WriteAllText(path, multilineText);
    }

    [Then(@"the environment variable ""(.*)"" is set to ""(.*)""")]
    public void ThenTheEnvironmentVariableIsSetTo(string name, string expectedValue)
    {
        var value = Environment.GetEnvironmentVariable(name);
        value.Should().Be(expectedValue);
    }

    [AfterScenario]
    public void CleanupFiles()
    {
        foreach (var file in this.files)
        {
            this.specFlowOutputHelper.WriteLine($"[EnvironmentVariablesSteps] Deleting file {file}");
            File.Delete(file);
        }
    }
}
