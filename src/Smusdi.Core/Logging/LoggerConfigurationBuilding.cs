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
        webApplicationBuilder.Host.UseSerilog(logger);

        return webApplicationBuilder;
    }
}
