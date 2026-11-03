using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Smusdi.Swagger;

internal sealed class SwaggerUIOptionsConfigurator : ISwaggerUIOptionsConfigurator
{
    public void Configure(SwaggerUIOptions options, IConfiguration configuration)
    {
        var oauthSection = configuration.GetSection("oauth");
        if (!oauthSection.Exists())
        {
            return;
        }

        options.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "nonce", "123456" } });
        options.OAuthUsePkce();

        if (configuration.GetValue<bool?>("oauth:hideClientSecretInputInSwaggerUi") ?? true)
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
}
