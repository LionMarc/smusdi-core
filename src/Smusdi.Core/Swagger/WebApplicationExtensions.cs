using System.Text;
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

                serverUrlBuilder.Append($"/{swaggerOptions.ReverseProxyBasePath ?? string.Empty}");

                var serverUrl = serverUrlBuilder.ToString();
                swaggerDoc.Servers = [new() { Url = serverUrl }];
            });
        });

        var oauthOptions = OauthOptions.GetOauthOptions(configuration);
        var smusdiOptions = SmusdiOptions.GetSmusdiOptions(configuration);

        webApplication.UseSwaggerUI(options =>
        {
            if (!string.IsNullOrWhiteSpace(swaggerOptions.DocumentTitle))
            {
                options.DocumentTitle = swaggerOptions.DocumentTitle;
            }

            if (smusdiOptions.NoVersioning != true)
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

            if (oauthOptions != null)
            {
                options.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "nonce", "123456" } });
                options.OAuthUsePkce();

                // https://github.com/swagger-api/swagger-ui/pull/8268 => client secret is now always visible in ui
                if (!swaggerOptions.DisplayClientSecretInput)
                {
                    var builder = new StringBuilder(options.HeadContent);
                    builder.Append(@"
<style>
label[for=""client_secret""] {
    display: none;
}
#client_secret {
    display: none
}
label[for=""client_secret_authorizationCode""] {
    display: none;
}
#client_secret_authorizationCode {
    display: none
}
</style>");
                    options.HeadContent = builder.ToString();
                }
            }
        });

        return webApplication;
    }
}
