namespace Smusdi.Core.Swagger;

public class SwaggerOptions
{
    public string Title { get; set; } = "SMuSDI Service";

    public string Description { get; set; } = "Default SMuSDI service";

    public List<string> Versions { get; set; } = new List<string>();

    public string ContactName { get; set; } = "SMuSDI";

    public string ContactMail { get; set; } = "SMuSDI@local.fr";

    public string ReverseProxyBasePath { get; set; } = string.Empty;

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
