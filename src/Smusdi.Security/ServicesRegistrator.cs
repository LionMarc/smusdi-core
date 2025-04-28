using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Extensibility;

namespace Smusdi.Security;

internal sealed class ServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<ISwaggerUIOptionsConfigurator, SwaggerUIOptionsConfigurator>()
            .AddSingleton<IIssuerSigningKeyResolver, IssuerSigningKeyResolver>()
            .AddHttpClient<ISigningKeysLoader, SigningKeysLoader>();

        return services;
    }
}
