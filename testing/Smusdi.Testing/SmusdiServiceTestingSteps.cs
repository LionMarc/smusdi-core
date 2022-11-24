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

    [AfterScenario]
    public void DisposeSmusdiTestingService()
    {
        this.smusdiTestingService.Dispose();
        Environment.SetEnvironmentVariable(SmusdiConstants.SmusdiAppsettingsFolderEnvVar, string.Empty);
    }

    [Given(@"the environment variable ""(.*)"" set to ""(.*)""")]
    public void GivenTheEnvironmentVariableSetTo(string p0, string p1)
    {
        Environment.SetEnvironmentVariable(p0, p1);
    }

    [Given(@"the service initialized")]
    public void GivenTheServiceInitialized()
    {
        SetEnvironmentIfNotSet();
        this.smusdiTestingService.Initialize();
    }

    [Given(@"the service initialized and started")]
    public async Task GivenTheServiceInitializedAndStarted()
    {
        SetEnvironmentIfNotSet();
        this.smusdiTestingService.Initialize();
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
