using BoDi;
using Smusdi.Core;
using TechTalk.SpecFlow;

namespace Smusdi.Testing;

[Binding]
public sealed class SmusdiServiceTestingSteps
{
    private readonly IObjectContainer objectContainer;
#pragma warning disable SA1000 // Keywords should be spaced correctly
    private readonly SmusdiTestingService smusdiTestingService = new();
#pragma warning restore SA1000 // Keywords should be spaced correctly

    public SmusdiServiceTestingSteps(IObjectContainer objectContainer)
    {
        this.objectContainer = objectContainer;
    }

    [BeforeScenario]
    public void RegisterSmusdiTestingService()
    {
        this.objectContainer.RegisterInstanceAs(this.smusdiTestingService);
    }

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
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "SpecFlow");
        this.smusdiTestingService.Initialize();
    }

    [Given(@"the service initialized and started")]
    public async Task GivenTheServiceInitializedAndStarted()
    {
        this.smusdiTestingService.Initialize();
        await this.smusdiTestingService.StartAsync();
    }

    [When(@"I start the service")]
    public Task WhenIStartTheService() => this.smusdiTestingService.StartAsync();
}
