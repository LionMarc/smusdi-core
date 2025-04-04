using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Smusdi.Extensibility;

public interface IBaseServicesRegistrator
{
    IServiceCollection Add(IServiceCollection services, IConfiguration configuration);
}
