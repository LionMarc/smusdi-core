using Microsoft.OpenApi.Models;
using Smusdi.Core.Oauth;

namespace Smusdi.Core.Swagger;

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
                if (!httpRequest.Headers.ContainsKey("X-Forwarded-Host") || string.IsNullOrWhiteSpace(swaggerOptions.ReverseProxyBasePath))
                {
                    return;
                }

                var serverUrl = $"{httpRequest.Scheme}://{httpRequest.Headers["X-Forwarded-Host"]}/{swaggerOptions.ReverseProxyBasePath}";
                swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
            });
        });

        var oauthOptions = OauthOptions.GetOauthOptions(configuration);

        webApplication.UseSwaggerUI(options =>
        {
            if (swaggerOptions.Versions.Count > 0)
            {
                options.RoutePrefix = "swagger";
                var descriptions = webApplication.DescribeApiVersions();
                foreach (var desc in descriptions)
                {
                    var url = $"v{desc.GroupName}/swagger.json";
                    var name = desc.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, $"V{name}");
                }
            }

            if (oauthOptions != null)
            {
                options.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "nonce", "123456" } });
            }
        });

        return webApplication;
    }
}
