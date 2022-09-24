using Microsoft.OpenApi.Models;
using Smusdi.Core.Oauth;

namespace Smusdi.Core.Swagger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var swaggerOptions = SwaggerOptions.GetSwaggerOptions(configuration);
        var openApiInfo = new OpenApiInfo()
        {
            Title = swaggerOptions.Title,
            Description = swaggerOptions.Description,
            Version = swaggerOptions.Version,
            Contact = new OpenApiContact()
            {
                Name = swaggerOptions.ContactName,
                Email = swaggerOptions.ContactMail,
            },
        };

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(swaggerOptions.Version, openApiInfo);
            options.AddSecurity(configuration);

            foreach (var xmlFile in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.xml"))
            {
                options.IncludeXmlComments(xmlFile);
            }
        });

        return services;
    }
}
