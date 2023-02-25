namespace Smusdi.Core.Configuration;

public static class ConfigurationBuilding
{
    public static WebApplicationBuilder InitConfiguration(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Configuration.InitConfiguration(webApplicationBuilder.Environment.EnvironmentName);
        return webApplicationBuilder;
    }

    public static HostApplicationBuilder InitConfiguration(this HostApplicationBuilder hostApplicationBuilder)
    {
        hostApplicationBuilder.Configuration.InitConfiguration(hostApplicationBuilder.Environment.EnvironmentName);
        return hostApplicationBuilder;
    }

    public static void InitConfiguration(this ConfigurationManager configuration, string environmentName)
    {
        // Cleanup already registered file config sources as the reset of the base path does not affect the sources, the associated provider is not reset!
        foreach (var fileSource in configuration.Sources.OfType<FileConfigurationSource>())
        {
            fileSource.FileProvider = null;
        }

        configuration.SetBasePath(GetConfigFilesFolder());

        var serviceName = Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiServiceNameEnvVar);
        if (!string.IsNullOrWhiteSpace(serviceName))
        {
            configuration
                .AddJsonFile($"appsettings.{serviceName}.json", true, true)
                .AddJsonFile($"appsettings.{serviceName}.{environmentName}.json", true, true);
        }

        configuration
            .EnableEnvironmentVariablesExpansion();
    }

    public static string GetConfigFilesFolder()
    {
        var basePath = Environment.ExpandEnvironmentVariables(Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiAppsettingsFolderEnvVar) ?? string.Empty);
        return Directory.Exists(basePath) ? Path.GetFullPath(basePath) : Directory.GetCurrentDirectory();
    }
}
