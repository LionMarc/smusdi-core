using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Smusdi.Extensibility;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Smusdi.Security;

public sealed class SwaggerUIOptionsConfigurator : ISwaggerUIOptionsConfigurator
{
    public void Configure(SwaggerUIOptions options, IConfiguration configuration)
    {
        var oauthOptions = OauthOptions.GetOauthOptions(configuration);
        if (oauthOptions == null)
        {
            return;
        }

        options.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "nonce", "123456" } });
        options.OAuthUsePkce();

        // https://github.com/swagger-api/swagger-ui/pull/8268 => client secret is now always visible in ui
        if (oauthOptions.HideClientSecretInputInSwaggerUi)
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
