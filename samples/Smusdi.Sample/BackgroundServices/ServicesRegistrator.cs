using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Core.Extensibility;

namespace Smusdi.Sample.BackgroundServices;

internal class ServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        return services.AddHostedService<HelloDeferredBackgroundService>()
            .AddHostedService<CoucoucDeferredBackgroundService>();
    }
}
