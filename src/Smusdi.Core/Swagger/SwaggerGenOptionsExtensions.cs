using Microsoft.OpenApi.Models;
using Smusdi.Core.Oauth;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Smusdi.Core.Swagger;

public static class SwaggerGenOptionsExtensions
{
    public static SwaggerGenOptions AddSecurity(this SwaggerGenOptions swaggerGenOptions, IConfiguration configuration)
    {
        var oauthOptions = OauthOptions.GetOauthOptions(configuration);
        if (oauthOptions == null)
        {
            return swaggerGenOptions;
        }

        swaggerGenOptions.AddSecurityDefinition(
            "oauth2",
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{oauthOptions.Authority}/protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{oauthOptions.Authority}/protocol/openid-connect/token"),
                        Scopes = new[] { "openid", "profile", "email" }.Concat(oauthOptions.Scopes ?? Enumerable.Empty<string>())
                        .ToDictionary(p => p, p => string.Empty),
                    },
                },
            });
        swaggerGenOptions.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "oauth2",
                                Type = ReferenceType.SecurityScheme,
                            },
                        },
                        new List<string>()
                    },
            });

        return swaggerGenOptions;
    }
}
