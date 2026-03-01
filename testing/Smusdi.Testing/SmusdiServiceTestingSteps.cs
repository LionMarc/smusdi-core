using System.Globalization;
using Microsoft.Extensions.Time.Testing;
using Reqnroll;
using Smusdi.Extensibility;

namespace Smusdi.Testing;

[Binding]
public sealed class SmusdiServiceTestingSteps(SmusdiTestingService smusdiTestingService, ScenarioContext scenarioContext)
{
    /// <summary>
    /// Order for the <see cref="BeforeScenarioAttribute"/> so the service initialization
    /// runs early in the hook pipeline.
    /// </summary>
    public const int ServiceInitializationHookOrder = HookAttribute.DefaultOrder - 500;

    public const string TargetTag = "integration";

    private readonly ScenarioContext scenarioContext = scenarioContext;

    /// <summary>
    /// Gets the test runner service that manages the in-memory Smusdi application.
    /// </summary>
    public SmusdiTestingService SmusdiTestingService => smusdiTestingService;

    /// <summary>
    /// Gets the command line arguments that will be passed to the tested application.
    /// </summary>
    public List<string> Args { get; } = [];

    /// <summary>
    /// Gets the actions to execute before starting the tested service (can be used by other steps).
    /// </summary>
    public List<Action> BeforeStarting { get; } = [];

    [BeforeScenario(TargetTag, Order = ServiceInitializationHookOrder)]
    public Task ServiceInitializationAndStart() => this.GivenTheServiceInitializedAndStarted();

    /// <summary>
    /// AfterScenario hook: disposes the test service and resets environment state.
    /// </summary>
    [AfterScenario]
    public void DisposeSmusdiTestingService()
    {
        this.SmusdiTestingService.Dispose();
        Environment.SetEnvironmentVariable(SmusdiConstants.SmusdiAppsettingsFolderEnvVar, string.Empty);
    }

    /// <summary>
    /// Given step: sets the command line arguments used to configure the tested application.
    /// </summary>
    /// <param name="dataTable">Table of arguments with columns 'Field' and 'Value'.</param>
    [Given("the command line arguments")]
    public void GivenTheCommandLineArguments(DataTable dataTable)
    {
        foreach (var row in dataTable.Rows)
        {
            this.Args.Add($"--{row["Field"]}={row["Value"]}");
        }
    }

    /// <summary>
    /// Given step: initializes the Smusdi service (without starting it).
    /// </summary>
    [Given(@"the service initialized")]
    public void GivenTheServiceInitialized()
    {
        SetEnvironmentIfNotSet();
        this.SetupCommandLineArguments();
        this.SmusdiTestingService.Initialize(this.Args.ToArray());
    }

    /// <summary>
    /// Given step: initializes and starts the Smusdi service for the scenario.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Given(@"the service initialized and started")]
    public async Task GivenTheServiceInitializedAndStarted()
    {
        this.GivenTheServiceInitialized();

        var tag = Array.Find(this.scenarioContext.ScenarioInfo.CombinedTags, p => p.StartsWith("withDateTime="));
        if (tag != null)
        {
            var datetime = tag.Split('=')[1];
            var fakeTimeProvider = this.SmusdiTestingService.GetRequiredService<TimeProvider>() as FakeTimeProvider;
            fakeTimeProvider?.SetUtcNow(DateTimeOffset.Parse(datetime, CultureInfo.InvariantCulture));
        }

        foreach (var action in this.BeforeStarting)
        {
            action();
        }

        await this.SmusdiTestingService.StartAsync();
    }

    [Given(@"the service started")]
    [When(@"I start the service")]
    public Task WhenIStartTheService() => this.SmusdiTestingService.StartAsync();

    private static void SetEnvironmentIfNotSet()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "reqnroll");
        }
    }

    private void SetupCommandLineArguments()
    {
        var args = new Dictionary<string, string>();

        // Tags on scenario can override tags on feature
        foreach (var tag in this.scenarioContext.ScenarioInfo.CombinedTags.Concat(this.scenarioContext.ScenarioInfo.Tags).Where(t => t.StartsWith("arg:")))
        {
            var value = tag.Substring(4);
            var parts = value.Split('=');
            if (parts.Length >= 1)
            {
                args[parts[0]] = string.Join("=", parts.Skip(1));
            }
        }

        foreach (var arg in args)
        {
            this.Args.Add($"--{arg.Key}={arg.Value}");
        }
    }
}
