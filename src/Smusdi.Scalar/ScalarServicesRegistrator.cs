using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Smusdi.Extensibility;

namespace Smusdi.Scalar;

internal sealed class ScalarServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        var scalarOptions = ScalarOptions.GetScalarOptions(configuration);
        if (scalarOptions.Title is null
            && scalarOptions.Description is null
            && scalarOptions.ContactEmail is null)
        {
            return services;
        }

        services.PostConfigureAll<OpenApiOptions>(options =>
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info ??= new OpenApiInfo();

                if (scalarOptions.Title is not null)
                {
                    document.Info.Title = scalarOptions.Title;
                }

                if (scalarOptions.Description is not null)
                {
                    document.Info.Description = scalarOptions.Description;
                }

                if (scalarOptions.ContactEmail is not null)
                {
                    document.Info.Contact ??= new OpenApiContact();
                    document.Info.Contact.Email = scalarOptions.ContactEmail;
                }

                return Task.CompletedTask;
            }));

        return services;
    }
}
