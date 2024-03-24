namespace Smusdi.Core.Configuration;

public static class ConfigurationBuilding
{
    public static WebApplicationBuilder InitConfiguration(this WebApplicationBuilder webApplicationBuilder, string[]? args)
    {
        webApplicationBuilder.Configuration.InitConfiguration(webApplicationBuilder.Environment.EnvironmentName, args);
        return webApplicationBuilder;
    }

    public static HostApplicationBuilder InitConfiguration(this HostApplicationBuilder hostApplicationBuilder, string[]? args)
    {
        hostApplicationBuilder.Configuration.InitConfiguration(hostApplicationBuilder.Environment.EnvironmentName, args);
        return hostApplicationBuilder;
    }

    public static void InitConfiguration(this ConfigurationManager configuration, string environmentName, string[]? args)
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

        if (args is { Length: > 0 })
        {
            // already done during default construction. But, the previous files may have overwritten these values.
            configuration.AddCommandLine(args);
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
