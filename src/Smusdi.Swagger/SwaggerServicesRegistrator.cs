using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Smusdi.Extensibility;

namespace Smusdi.Swagger;

internal sealed class SwaggerServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        var swaggerOptions = SwaggerOptions.GetSwaggerOptions(configuration);
        services.AddSwaggerUI();

        if (swaggerOptions.Title is null
            && swaggerOptions.Description is null
            && swaggerOptions.ContactEmail is null)
        {
            return services;
        }

        services.PostConfigureAll<OpenApiOptions>(options =>
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info ??= new OpenApiInfo();

                if (swaggerOptions.Title is not null)
                {
                    document.Info.Title = swaggerOptions.Title;
                }

                if (swaggerOptions.Description is not null)
                {
                    document.Info.Description = swaggerOptions.Description;
                }

                if (swaggerOptions.ContactEmail is not null)
                {
                    document.Info.Contact ??= new OpenApiContact();
                    document.Info.Contact.Email = swaggerOptions.ContactEmail;
                }

                return Task.CompletedTask;
            }));

        return services;
    }
}
