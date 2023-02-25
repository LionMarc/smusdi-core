using Serilog;
using Smusdi.Core.Configuration;

namespace Smusdi.Core.Logging;

public static class LoggerConfigurationBuilding
{
    public static WebApplicationBuilder InitLoggerConfiguration(this WebApplicationBuilder webApplicationBuilder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(webApplicationBuilder.Configuration)
            .CreateLogger();

        logger.Information($"Configuration loaded from folder {ConfigurationBuilding.GetConfigFilesFolder()}.");

        webApplicationBuilder.Services.AddSingleton(logger);
        webApplicationBuilder.Services.AddSingleton<Serilog.ILogger>(logger);
        webApplicationBuilder.Host.UseSerilog(logger);

        return webApplicationBuilder;
    }

    public static HostApplicationBuilder InitLoggerConfiguration(this HostApplicationBuilder hostApplicationBuilder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(hostApplicationBuilder.Configuration)
            .CreateLogger();

        logger.Information($"Configuration loaded from folder {ConfigurationBuilding.GetConfigFilesFolder()}.");

        hostApplicationBuilder.Services.AddSingleton(logger);
        hostApplicationBuilder.Services.AddSingleton<Serilog.ILogger>(logger);
        hostApplicationBuilder.Logging.ClearProviders();
        hostApplicationBuilder.Logging.AddSerilog(logger);

        return hostApplicationBuilder;
    }
}
