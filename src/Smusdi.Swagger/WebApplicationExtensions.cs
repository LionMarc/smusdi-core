using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Smusdi.Swagger;

public static class WebApplicationExtensions
{
    public static WebApplication UseSwaggerUI(this WebApplication webApplication, IConfiguration configuration)
    {
        var swaggerOptions = SwaggerOptions.GetSwaggerOptions(configuration);

        var noVersioning = configuration.GetValue<bool?>("smusdi:NoVersioning") == true;
        var swaggerUIOptionsSetters = webApplication.Services.GetServices<ISwaggerUIOptionsConfigurator>().ToList();

        webApplication.UseSwaggerUI(options =>
        {
            if (!string.IsNullOrWhiteSpace(swaggerOptions.DocumentTitle))
            {
                options.DocumentTitle = swaggerOptions.DocumentTitle;
            }

            if (noVersioning)
            {
                options.SwaggerEndpoint("../openapi/v1.json", "V1");
            }
            else
            {
                var descriptions = webApplication.DescribeApiVersions();
                foreach (var desc in descriptions.Select(d => d.GroupName))
                {
                    var url = $"../openapi/{desc}.json";
                    var name = desc.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            }

            swaggerUIOptionsSetters.ForEach(setter => setter.Configure(options, configuration));
        });

        return webApplication;
    }
}
