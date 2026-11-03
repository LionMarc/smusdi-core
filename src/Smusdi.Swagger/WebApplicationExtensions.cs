using System.Text;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Smusdi.Swagger;

public static class WebApplicationExtensions
{
    public static WebApplication UseSwagger(this WebApplication webApplication, IConfiguration configuration)
    {
        var swaggerOptions = SwaggerOptions.GetSwaggerOptions(configuration);

        webApplication.UseSwagger(options =>
        {
            options.RouteTemplate = "swagger/{documentName}/swagger.json";

            options.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
            {
                if (!httpRequest.Headers.TryGetValue("X-Forwarded-Host", out var forwardedHost))
                {
                    return;
                }

                var scheme = httpRequest.Scheme;
                if (httpRequest.Headers.TryGetValue("X-Forwarded-Proto", out var forwardedScheme))
                {
                    scheme = forwardedScheme;
                }

                int? port = null;
                if (swaggerOptions.IncludeForwardedPort)
                {
                    port = httpRequest.Host.Port;
                    if (httpRequest.Headers.TryGetValue("X-Forwarded-Port", out var forwardedPort) && int.TryParse(forwardedPort, out var forwardedPortValue))
                    {
                        port = forwardedPortValue;
                    }
                }

                var serverUrlBuilder = new StringBuilder();
                serverUrlBuilder.Append($"{scheme}://{forwardedHost}");
                if (port.HasValue)
                {
                    serverUrlBuilder.Append($"{port.Value}");
                }

                serverUrlBuilder.Append($"/{swaggerOptions.ReverseProxyBasePath}");

                swaggerDoc.Servers = [new() { Url = serverUrlBuilder.ToString() }];
            });
        });

        var noVersioning = configuration.GetValue<bool?>("smusdi:NoVersioning") == true;
        var swaggerUIOptionsSetters = webApplication.Services.GetServices<ISwaggerUIOptionsConfigurator>().ToList();

        webApplication.UseSwaggerUI(options =>
        {
            if (!string.IsNullOrWhiteSpace(swaggerOptions.DocumentTitle))
            {
                options.DocumentTitle = swaggerOptions.DocumentTitle;
            }

            if (!noVersioning)
            {
                options.RoutePrefix = "swagger";
                var descriptions = webApplication.DescribeApiVersions();
                foreach (var desc in descriptions.Select(d => d.GroupName))
                {
                    var url = $"{desc}/swagger.json";
                    var name = desc.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            }

            swaggerUIOptionsSetters.ForEach(setter => setter.Configure(options, configuration));
        });

        return webApplication;
    }
}
