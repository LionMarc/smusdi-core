using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi;
using Serilog;
using Smusdi.Extensibility;

namespace Smusdi.Security;

public sealed class OAuthConfigurator : ISecurityConfigurator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        var oauthOptions = OauthOptions.GetOauthOptions(configuration);
        var authenticationBuilder = services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = SecurityConstants.SelectionScheme;
                x.DefaultChallengeScheme = SecurityConstants.SelectionScheme;
            });

        foreach (var authority in oauthOptions.AdditionalAuthorities.Concat([oauthOptions.MainAuthority]))
        {
            authenticationBuilder = authority.AddJwtBearer(authenticationBuilder);

            if (authority.Type == OauthAuthorityType.Jwks)
            {
                services.AddOptions<JwtBearerOptions>(authority.Name)
                    .Configure<IIssuerSigningKeyResolver>((options, issuerSigningKeyResolver) =>
                    {
                        options.TokenValidationParameters.IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                            issuerSigningKeyResolver.GetIssuerSigningKeys(authority.Url, kid, authority.CacheLifespan ?? OauthAuthority.DefaultCacheLifespan);
                    });
            }
        }

        authenticationBuilder.AddPolicyScheme(SecurityConstants.SelectionScheme, SecurityConstants.SelectionScheme, options =>
        {
            options.ForwardDefaultSelector = context =>
            {
                // If no additional authority, return the main authority without checking the token and the headers.
                if (oauthOptions.AdditionalAuthorities.Count == 0)
                {
                    return oauthOptions.MainAuthority.Name;
                }

                var logger = context.RequestServices.GetRequiredService<ILogger>();
                logger.Debug("[Security] Selecting authority...");
                var authorities = oauthOptions.GetAllAuthorities();

                // Try to check if a custom header is defined and set
                var selected = authorities
                        .FirstOrDefault(a => !string.IsNullOrWhiteSpace(a.AuthorizationHeader) && context.Request.Headers.ContainsKey(a.AuthorizationHeader));
                if (selected != null)
                {
                    var debug = $"[Security] Selecting {selected.Name} with header {selected.AuthorizationHeader}.";
                    logger.Debug(debug);
                    return selected.Name;
                }

                // If not check the standard header
                string? authorization = context.Request.Headers[HeaderNames.Authorization];
                if (!string.IsNullOrWhiteSpace(authorization))
                {
                    var token = authorization;
                    if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        token = token["Bearer ".Length..].Trim();
                    }

                    var jwtHandler = new JwtSecurityTokenHandler();
                    if (jwtHandler.CanReadToken(token))
                    {
                        var issuer = jwtHandler.ReadJwtToken(token).Issuer;
                        var authority = authorities.FirstOrDefault(a => string.IsNullOrWhiteSpace(a.AuthorizationHeader) && issuer.Equals(a.Issuer ?? a.Url));
                        if (authority != null)
                        {
                            return authority.Name;
                        }
                    }
                    else
                    {
                        logger.Warning("[Security] Unable to read token. Defaulting to main authority.");
                    }
                }

                return oauthOptions.MainAuthority.Name;
            };
        });

        // Define policies for each scope
        services.AddAuthorization(options =>
        {
            foreach (var scope in oauthOptions.Scopes ?? Enumerable.Empty<string>())
            {
                options.AddPolicy(
                    scope,
                    policybuilder => policybuilder.RequireAssertion(context => context.User.Claims.FirstOrDefault(c => c.Type == "scope")?.Value.Contains(scope) == true));
            }
        });

        services.ConfigureAll<OpenApiOptions>(options => options.AddDocumentTransformer((document, _, _) =>
        {
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
            document.Security ??= [];

            if (!string.IsNullOrWhiteSpace(oauthOptions.MainAuthority?.Url))
            {
                document.Components.SecuritySchemes["oauth2"] = GetSecuritySchema(oauthOptions.MainAuthority, oauthOptions.Scopes);
                document.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("oauth2", document)] = [],
                });
                AddOpenIdConnectSecuritySchema(document, "oauth2", oauthOptions.MainAuthority);
            }

            foreach (var authority in oauthOptions.AdditionalAuthorities)
            {
                document.Components.SecuritySchemes[authority.Name] = GetSecuritySchema(authority, oauthOptions.Scopes);
                document.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference(authority.Name, document)] = [],
                });
                AddOpenIdConnectSecuritySchema(document, authority.Name, authority);
            }

            return Task.CompletedTask;
        }));

        return services;
    }

    public IApplicationBuilder Configure(IApplicationBuilder app, IConfiguration configuration)
    {
        var oauthOptions = OauthOptions.GetOauthOptions(configuration);
        if (oauthOptions == null)
        {
            return app;
        }

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    private static OpenApiSecurityScheme GetSecuritySchema(OauthAuthority authority, IEnumerable<string>? scopes)
    {
        const string jwksPath = "/protocol/openid-connect/certs";
        var authorityUrl = authority.Url.TrimEnd('/');
        if (authority.Type == OauthAuthorityType.Jwks && authorityUrl.EndsWith(jwksPath, StringComparison.OrdinalIgnoreCase))
        {
            authorityUrl = authorityUrl[..^jwksPath.Length];
        }

        return new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri($"{authorityUrl}/protocol/openid-connect/auth"),
                    TokenUrl = new Uri($"{authorityUrl}/protocol/openid-connect/token"),
                    Scopes = new[] { "openid", "profile", "email" }.Concat(scopes ?? Enumerable.Empty<string>())
                        .ToDictionary(scope => scope, _ => string.Empty),
                },
            },
        };
    }

    private static void AddOpenIdConnectSecuritySchema(OpenApiDocument document, string securitySchemeName, OauthAuthority authority)
    {
        var metadataUrl = GetOpenIdConnectUrl(authority);
        if (metadataUrl is null)
        {
            return;
        }

        var openIdConnectSchemeName = $"{securitySchemeName}-openid-connect";
        var components = document.Components ??= new OpenApiComponents();
        components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        components.SecuritySchemes[openIdConnectSchemeName] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OpenIdConnect,
            OpenIdConnectUrl = metadataUrl,
        };
        var securityRequirements = document.Security ??= [];
        securityRequirements.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(openIdConnectSchemeName, document)] = [],
        });
    }

    private static Uri? GetOpenIdConnectUrl(OauthAuthority authority)
    {
        if (!string.IsNullOrWhiteSpace(authority.OpenIdConnectUrl))
        {
            if (Uri.TryCreate(authority.OpenIdConnectUrl, UriKind.Absolute, out var configuredUrl)
                && (configuredUrl.Scheme == Uri.UriSchemeHttp || configuredUrl.Scheme == Uri.UriSchemeHttps))
            {
                return configuredUrl;
            }

            throw new InvalidOperationException(
                $"The OpenID Connect URL configured for authority '{authority.Name}' must be an absolute HTTP or HTTPS URL.");
        }

        if (authority.Type == OauthAuthorityType.Custom)
        {
            return null;
        }

        const string jwksPath = "/protocol/openid-connect/certs";
        var authorityUrl = authority.Url.TrimEnd('/');
        if (authority.Type == OauthAuthorityType.Jwks)
        {
            if (!authorityUrl.EndsWith(jwksPath, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            authorityUrl = authorityUrl[..^jwksPath.Length];
        }

        return new Uri($"{authorityUrl}/.well-known/openid-configuration");
    }
}
