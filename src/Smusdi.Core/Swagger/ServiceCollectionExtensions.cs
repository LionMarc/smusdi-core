using Microsoft.OpenApi.Models;
using Smusdi.Core.Oauth;

namespace Smusdi.Core.Swagger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var swaggerOptions = SwaggerOptions.GetSwaggerOptions(configuration);

        services.AddSwaggerGen(options =>
        {
            foreach (var version in swaggerOptions.Versions)
            {
                var openApiInfo = new OpenApiInfo()
                {
                    Title = swaggerOptions.Title,
                    Description = swaggerOptions.Description,
                    Version = version,
                    Contact = new OpenApiContact()
                    {
                        Name = swaggerOptions.ContactName,
                        Email = swaggerOptions.ContactMail,
                    },
                };
                options.SwaggerDoc(version, openApiInfo);
            }

            options.AddSecurity(configuration);

            foreach (var xmlFile in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.xml"))
            {
                options.IncludeXmlComments(xmlFile);
            }
        });

        return services;
    }
}
