using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Smusdi.Core.Extensibility;

namespace Smusdi.Sample.BeforeRun;

internal class ServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        return services.AddScoped<ITestingService, TestingService>();
    }
}
