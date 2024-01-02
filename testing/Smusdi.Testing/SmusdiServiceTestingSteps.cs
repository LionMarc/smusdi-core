using Smusdi.Core;
using TechTalk.SpecFlow;

namespace Smusdi.Testing;

[Binding]
public sealed class SmusdiServiceTestingSteps
{
    private readonly SmusdiTestingService smusdiTestingService;

    public SmusdiServiceTestingSteps(SmusdiTestingService smusdiTestingService)
    {
        this.smusdiTestingService = smusdiTestingService;
    }

    public SmusdiTestingService SmusdiTestingService => this.smusdiTestingService;

    public List<string> Args { get; } = new();

    [AfterScenario]
    public void DisposeSmusdiTestingService()
    {
        this.smusdiTestingService.Dispose();
        Environment.SetEnvironmentVariable(SmusdiConstants.SmusdiAppsettingsFolderEnvVar, string.Empty);
    }

    [Given(@"the service initialized")]
    public void GivenTheServiceInitialized()
    {
        SetEnvironmentIfNotSet();
        this.smusdiTestingService.Initialize(this.Args.ToArray());
    }

    [Given(@"the service initialized and started")]
    public async Task GivenTheServiceInitializedAndStarted()
    {
        SetEnvironmentIfNotSet();
        this.smusdiTestingService.Initialize(this.Args.ToArray());
        await this.smusdiTestingService.StartAsync();
    }

    [When(@"I start the service")]
    public Task WhenIStartTheService() => this.smusdiTestingService.StartAsync();

    private static void SetEnvironmentIfNotSet()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "SpecFlow");
        }
    }
}
