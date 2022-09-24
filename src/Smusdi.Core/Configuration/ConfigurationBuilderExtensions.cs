namespace Smusdi.Core.Configuration;

internal static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder EnableEnvironmentVariablesExpansion(this IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Build();
        return configurationBuilder.Add(new EnvironementVariablesExpanderSource());
    }
}
