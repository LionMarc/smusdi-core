using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Smusdi.Security;

public static class SwaggerGenOptionsExtensions
{
    public static SwaggerGenOptions AddSecurity(this SwaggerGenOptions swaggerGenOptions, IConfiguration configuration)
    {
        var oauthOptions = OauthOptions.GetOauthOptions(configuration);
        var requirement = new OpenApiSecurityRequirement();
        if (!string.IsNullOrWhiteSpace(oauthOptions.MainAuthority?.Url))
        {
            swaggerGenOptions.AddSecurityDefinition(
                "oauth2",
                GetSecutirySchema(oauthOptions.MainAuthority.Url, oauthOptions.Scopes));
            requirement.Add(
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "oauth2",
                        Type = ReferenceType.SecurityScheme,
                    },
                },
                []);
        }

        foreach (var authority in oauthOptions.AdditionalAuthorities)
        {
            swaggerGenOptions.AddSecurityDefinition(authority.Name, GetSecutirySchema(authority.Url, oauthOptions.Scopes));
            requirement.Add(
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = authority.Name,
                        Type = ReferenceType.SecurityScheme,
                    },
                },
                []);
        }

        swaggerGenOptions.AddSecurityRequirement(requirement);

        return swaggerGenOptions;
    }

    public static IApplicationBuilder SetupSwaggerUI(this IApplicationBuilder app, OauthOptions oauthOptions)
    {
        app.UseSwaggerUI(options =>
        {
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
        });

        return app;
    }

    private static OpenApiSecurityScheme GetSecutirySchema(string url, IEnumerable<string>? scopes) => new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{url}/protocol/openid-connect/auth"),
                TokenUrl = new Uri($"{url}/protocol/openid-connect/token"),
                Scopes = new[] { "openid", "profile", "email" }.Concat(scopes ?? Enumerable.Empty<string>())
                        .ToDictionary(p => p, p => string.Empty),
            },
        },
    };
}
