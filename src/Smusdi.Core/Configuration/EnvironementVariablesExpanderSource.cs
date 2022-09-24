namespace Smusdi.Core.Configuration;

public class EnvironementVariablesExpanderSource : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new EnvironmentVariablesExpanderProvider(builder);
    }
}
