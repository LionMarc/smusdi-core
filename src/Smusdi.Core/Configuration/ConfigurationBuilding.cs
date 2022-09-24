namespace Smusdi.Core.Configuration;

public static class ConfigurationBuilding
{
    public static WebApplicationBuilder InitConfiguration(this WebApplicationBuilder webApplicationBuilder)
    {
        // Cleanup already registered file config sources as the reset of the base path does not affect the sources, the associated provider is not reset!
        foreach (var fileSource in (webApplicationBuilder.Configuration as IConfigurationBuilder).Sources.OfType<FileConfigurationSource>())
        {
            fileSource.FileProvider = null;
        }

        webApplicationBuilder.Configuration
            .SetBasePath(GetConfigFilesFolder());

        var serviceName = Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiServiceNameEnvVar);
        if (!string.IsNullOrWhiteSpace(serviceName))
        {
            webApplicationBuilder.Configuration
                .AddJsonFile($"appsettings.{serviceName}.json", true, true)
                .AddJsonFile($"appsettings.{serviceName}.{webApplicationBuilder.Environment.EnvironmentName}.json", true, true);
        }

        webApplicationBuilder.Configuration
            .EnableEnvironmentVariablesExpansion();

        return webApplicationBuilder;
    }

    public static string GetConfigFilesFolder()
    {
        var basePath = Environment.ExpandEnvironmentVariables(Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiAppsettingsFolderEnvVar) ?? string.Empty);
        return Directory.Exists(basePath) ? Path.GetFullPath(basePath) : Directory.GetCurrentDirectory();
    }
}
