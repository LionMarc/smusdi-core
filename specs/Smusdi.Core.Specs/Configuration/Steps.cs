using Microsoft.Extensions.Configuration;
using Reqnroll;
using Smusdi.Testing;

namespace Smusdi.Core.Specs.Configuration;

[Binding]
public sealed class Steps(SmusdiServiceTestingSteps smusdiServiceTestingSteps)
{
    private readonly SmusdiTestingService smusdiTestingService = smusdiServiceTestingSteps.SmusdiTestingService;

    [AfterScenario]
    public static void Cleanup()
    {
        Environment.SetEnvironmentVariable("SMUSDI_APPSETTINGS_FOLDER", null);

        foreach (var file in new[] { "appsettings.json", "appsettings.myservice.json" })
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }

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

    [Given(@"the configuration file ""(.*)""")]
    public void GivenTheConfigurationFile(string appsettingsFilename, string multilineText)
    {
        File.WriteAllText(appsettingsFilename, multilineText);
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

    [Then("the configuration property {string} of the section {string} is equal to {string}")]
    public void ThenTheConfigurationPropertyOfTheSectionIsEqualTo(string propertyName, string sectionName, string value)
    {
        var configuration = this.smusdiTestingService.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(sectionName);
        var property = section[propertyName];
        property.Should().Be(value);
    }
}
