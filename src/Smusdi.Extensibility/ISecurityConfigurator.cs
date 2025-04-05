using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Smusdi.Extensibility;

public interface ISecurityConfigurator : IBaseServicesRegistrator
{
    IApplicationBuilder Configure(IApplicationBuilder app, IConfiguration configuration);
}
