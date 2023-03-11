using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Core.Extensibility;
using Smusdi.Core.Helpers;
using Smusdi.Core.Worker;

namespace Smusdi.Worker.Sample;

internal sealed class ServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSecuredHttpClient<ISampleService, SampleService>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:5100/v1/");
        });

        return services;
    }
}
