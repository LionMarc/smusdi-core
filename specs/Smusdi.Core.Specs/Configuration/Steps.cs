using Smusdi.Testing;

namespace Smusdi.Core.Specs.Configuration;

[Binding]
public sealed class Steps
{
    private readonly SmusdiTestingService smusdiTestingService;

    public Steps(SmusdiTestingService smusdiTestingService) => this.smusdiTestingService = smusdiTestingService;

    [Given(@"the configuration in current folder")]
    public void GivenTheConfigurationInCurrentFolder(string multilineText)
    {
        File.WriteAllText("appsettings.json", multilineText);
    }

    [Given(@"the configuration in folder '(.*)'")]
    public void GivenTheConfigurationInFolder(string p0, string multilineText)
    {
        Directory.CreateDirectory(p0);
        File.WriteAllText(Path.Combine(p0, "appsettings.json"), multilineText);
    }

    [Then(@"the read folder parameter is ""(.*)""")]
    public void ThenTheReadFolderParameterIs(string expectedFolder)
    {
        var read = this.smusdiTestingService.SmusdiService?.WebApplication?.Configuration["folder"];
        read.Should().Be(expectedFolder);
    }

    [Then(@"the value of the config property ""(.*)"" is ""(.*)""")]
    public void ThenTheValueOfTheConfigPropertyIs(string p0, string p1)
    {
        var read = this.smusdiTestingService.SmusdiService?.WebApplication?.Configuration[p0];
        read.Should().Be(p1);
    }
}
