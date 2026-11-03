using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Smusdi.Swagger;

public interface ISwaggerUIOptionsConfigurator
{
    void Configure(SwaggerUIOptions options, IConfiguration configuration);
}
