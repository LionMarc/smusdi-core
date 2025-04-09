using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
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

        // Secutiry for Swagger
        services.AddSwaggerGen(options => options.AddSecurity(configuration));

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

        app.SetupSwaggerUI(oauthOptions);

        return app;
    }
}
