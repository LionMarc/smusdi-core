using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Smusdi.Core.Swagger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var smusdiOptions = SmusdiOptions.GetSmusdiOptions(configuration);
        if (smusdiOptions.NoVersioning != true)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }

        services.AddSwaggerGen(options =>
        {
            if (smusdiOptions.NoVersioning == true)
            {
                var swaggerOptions = SwaggerOptions.GetSwaggerOptions(configuration);
                var openApiInfo = new OpenApiInfo()
                {
                    Title = swaggerOptions.Title,
                    Description = swaggerOptions.Description,
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = swaggerOptions.ContactName,
                        Email = swaggerOptions.ContactMail,
                    },
                };
                options.SwaggerDoc("v1", openApiInfo);
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
