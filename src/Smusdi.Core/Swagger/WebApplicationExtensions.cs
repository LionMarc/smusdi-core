using System.Text;
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
</style>");
                    options.HeadContent = builder.ToString();
                }
            }
        });

        return webApplication;
    }
}
