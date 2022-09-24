namespace Smusdi.Core.Swagger;

public class SwaggerOptions
{
    public string Title { get; set; } = "SMuSDI Service";

    public string Description { get; set; } = "Default SMuSDI service";

    public string Version { get; set; } = "v1";

    public string ContactName { get; set; } = "SMuSDI";

    public string ContactMail { get; set; } = "SMuSDI@local.fr";

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
