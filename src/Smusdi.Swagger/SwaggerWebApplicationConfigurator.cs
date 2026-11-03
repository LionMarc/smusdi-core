using Microsoft.AspNetCore.Builder;
using Smusdi.Extensibility;

namespace Smusdi.Swagger;

internal sealed class SwaggerWebApplicationConfigurator : IWebApplicationConfigurator
{
    public WebApplication Configure(WebApplication webApplication) => webApplication.UseSwaggerUI(webApplication.Configuration);
}
