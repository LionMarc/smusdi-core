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

        var requirement = new OpenApiSecurityRequirement();
        if (!string.IsNullOrWhiteSpace(oauthOptions.Authority))
        {
            swaggerGenOptions.AddSecurityDefinition(
                "oauth2",
                GetSecutirySchema(oauthOptions.Authority, oauthOptions.Scopes));
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
