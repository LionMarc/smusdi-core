using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Smusdi.Extensibility;

public interface ISwaggerUIOptionsConfigurator
{
    void Configure(SwaggerUIOptions options, IConfiguration configuration);
}
