﻿using System.Globalization;
using Microsoft.Extensions.Time.Testing;
using Reqnroll;
using Smusdi.Core;
using Smusdi.Extensibility;

namespace Smusdi.Testing;

[Binding]
public sealed class SmusdiServiceTestingSteps(SmusdiTestingService smusdiTestingService, ScenarioContext scenarioContext)
{
    private readonly ScenarioContext scenarioContext = scenarioContext;

    public SmusdiTestingService SmusdiTestingService => smusdiTestingService;

    public List<string> Args { get; } = new();

    [AfterScenario]
    public void DisposeSmusdiTestingService()
    {
        this.SmusdiTestingService.Dispose();
        Environment.SetEnvironmentVariable(SmusdiConstants.SmusdiAppsettingsFolderEnvVar, string.Empty);
    }

    [Given("the command line arguments")]
    public void GivenTheCommandLineArguments(DataTable dataTable)
    {
        foreach (var row in dataTable.Rows)
        {
            this.Args.Add($"--{row["Field"]}={row["Value"]}");
        }
    }

    [Given(@"the service initialized")]
    public void GivenTheServiceInitialized()
    {
        SetEnvironmentIfNotSet();
        this.SmusdiTestingService.Initialize(this.Args.ToArray());
    }

    [Given(@"the service initialized and started")]
    public async Task GivenTheServiceInitializedAndStarted()
    {
        SetEnvironmentIfNotSet();
        this.SmusdiTestingService.Initialize(this.Args.ToArray());

        var tag = Array.Find(this.scenarioContext.ScenarioInfo.CombinedTags, p => p.StartsWith("withDateTime="));
        if (tag != null)
        {
            var datetime = tag.Split('=')[1];
            var fakeTimeProvider = this.SmusdiTestingService.GetRequiredService<TimeProvider>() as FakeTimeProvider;
            fakeTimeProvider?.SetUtcNow(DateTimeOffset.Parse(datetime, CultureInfo.InvariantCulture));
        }

        await this.SmusdiTestingService.StartAsync();
    }

    [When(@"I start the service")]
    public Task WhenIStartTheService() => this.SmusdiTestingService.StartAsync();

    private static void SetEnvironmentIfNotSet()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "SpecFlow");
        }
    }
}
