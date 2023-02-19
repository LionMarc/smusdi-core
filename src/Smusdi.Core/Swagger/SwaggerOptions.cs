namespace Smusdi.Core.Swagger;

public class SwaggerOptions
{
    public string Title { get; set; } = "Smusdi Service";

    public string Description { get; set; } = "Default Smusdi service";

    public string ContactName { get; set; } = "Smusdi";

    public string ContactMail { get; set; } = "Smusdi@local.fr";

    public string ReverseProxyBasePath { get; set; } = string.Empty;

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
