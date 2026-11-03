using Microsoft.Extensions.Configuration;

namespace Smusdi.Swagger;

public class SwaggerOptions
{
    public string? DocumentTitle { get; set; }

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
