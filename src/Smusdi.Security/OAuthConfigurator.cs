using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
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
            authenticationBuilder = authenticationBuilder.AddJwtBearer(
                authority.Name,
                x =>
                {
                    x.Audience = authority.Audience;
                    x.Authority = authority.Url;
                    x.RequireHttpsMetadata = authority.RequireHttpsMetadata;
                });
        }

        authenticationBuilder.AddPolicyScheme(SecurityConstants.SelectionScheme, SecurityConstants.SelectionScheme, options =>
        {
            options.ForwardDefaultSelector = context =>
            {
                string? authorization = context.Request.Headers[HeaderNames.Authorization];
                if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                {
                    var token = authorization["Bearer ".Length..].Trim();
                    var jwtHandler = new JwtSecurityTokenHandler();
                    if (jwtHandler.CanReadToken(token))
                    {
                        var issuer = jwtHandler.ReadJwtToken(token).Issuer;
                        var authority = oauthOptions.AdditionalAuthorities.FirstOrDefault(a => issuer.Equals(a.Url));
                        if (authority != null)
                        {
                            return authority.Name;
                        }
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
