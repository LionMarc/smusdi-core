namespace Smusdi.Core.Swagger;

public class SwaggerOptions
{
    public string? DocumentTitle { get; set; } = null;

    public string Title { get; set; } = "Smusdi Service";

    public string Description { get; set; } = "Default Smusdi service";

    public string ContactName { get; set; } = "Smusdi";

    public string ContactMail { get; set; } = "Smusdi@local.fr";

    public string ReverseProxyBasePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether or not we must include the port in the built url when behind a revers proxy.
    /// </summary>
    public bool IncludeForwardedPort { get; set; } = false;

    public bool DisplayClientSecretInput { get; set; } = false;

    public static SwaggerOptions GetSwaggerOptions(IConfiguration configuration)
    {
        SwaggerOptions swaggerOptions = new SwaggerOptions();
        if (configuration.GetSection("swagger").Exists())
        {
            configuration.Bind("swagger", swaggerOptions);
        }

        return swaggerOptions;
    }
}
