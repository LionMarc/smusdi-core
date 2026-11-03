using Microsoft.Extensions.Configuration;

namespace Smusdi.Swagger;

public class SwaggerOptions
{
    public string? DocumentTitle { get; set; }

    public string Title { get; set; } = "Smusdi Service";

    public string Description { get; set; } = "Default Smusdi service";

    public string ContactName { get; set; } = "Smusdi";

    public string ContactMail { get; set; } = "Smusdi@local.fr";

    public string ReverseProxyBasePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether or not we must include the port in the built url when behind a reverse proxy.
    /// </summary>
    public bool IncludeForwardedPort { get; set; }

    public bool DisplayClientSecretInput { get; set; }

    public static SwaggerOptions GetSwaggerOptions(IConfiguration configuration)
    {
        var swaggerOptions = new SwaggerOptions();
        if (configuration.GetSection("swagger").Exists())
        {
            configuration.Bind("swagger", swaggerOptions);
        }

        return swaggerOptions;
    }
}
