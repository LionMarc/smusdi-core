using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Extensibility;

namespace Smusdi.Swagger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerUI(this IServiceCollection services)
    {
        services.AddSingleton<ISwaggerUIOptionsConfigurator, SwaggerUIOptionsConfigurator>();

        return services;
    }
}
