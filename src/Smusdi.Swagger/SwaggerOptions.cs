using Microsoft.Extensions.Configuration;

namespace Smusdi.Swagger;

public class SwaggerOptions
{
    public string? DocumentTitle { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ContactEmail { get; set; }

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
