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
                if (!httpRequest.Headers.TryGetValue("X-Forwarded-Host", out var forwardedHost))
                {
                    return;
                }

                var scheme = httpRequest.Scheme;
                if (httpRequest.Headers.TryGetValue("X-Forwarded-Proto", out var forwardedScheme))
                {
                    scheme = forwardedScheme;
                }

                var serverUrl = $"{scheme}://{forwardedHost}/{swaggerOptions.ReverseProxyBasePath ?? string.Empty}";
                swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
            });
        });

        var oauthOptions = OauthOptions.GetOauthOptions(configuration);
        var smusdiOptions = SmusdiOptions.GetSmusdiOptions(configuration);

        webApplication.UseSwaggerUI(options =>
        {
            if (smusdiOptions.NoVersioning != true)
            {
                options.RoutePrefix = "swagger";
                var descriptions = webApplication.DescribeApiVersions();
                foreach (var desc in descriptions)
                {
                    var url = $"{desc.GroupName}/swagger.json";
                    var name = desc.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
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
