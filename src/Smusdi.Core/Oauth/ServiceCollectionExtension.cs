using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;

namespace Smusdi.Core.Oauth;

public static class ServiceCollectionExtension
{
    public const string SmusdiDefaultScheme = "smusdi_default";
    public const string SelectionScheme = "selection";

    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var oauthOptions = OauthOptions.GetOauthOptions(configuration);
        if (oauthOptions == null)
        {
            return services;
        }

        var authenticationBuilder = services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = SelectionScheme;
                x.DefaultChallengeScheme = SelectionScheme;
            })
           .AddJwtBearer(
                SmusdiDefaultScheme,
                x =>
                {
                    x.Audience = oauthOptions.Audience;
                    x.Authority = oauthOptions.Authority;
                    x.RequireHttpsMetadata = false;
                });

        foreach (var authority in oauthOptions.AdditionalAuthorities)
        {
            authenticationBuilder = authenticationBuilder.AddJwtBearer(
                authority.Name,
                x =>
                {
                    x.Audience = authority.Audience;
                    x.Authority = authority.Url;
                    x.RequireHttpsMetadata = false;
                });
        }

        authenticationBuilder.AddPolicyScheme(SelectionScheme, SelectionScheme, options =>
         {
             options.ForwardDefaultSelector = context =>
             {
                 string? authorization = context.Request.Headers[HeaderNames.Authorization];
                 if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                 {
                     var token = authorization.Substring("Bearer ".Length).Trim();
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

                 return SmusdiDefaultScheme;
             };
         });

        services.AddAuthorization(options =>
            {
                foreach (var scope in oauthOptions.Scopes ?? Enumerable.Empty<string>())
                {
                    options.AddPolicy(
                        scope,
                        policybuilder => policybuilder.RequireAssertion(context => context.User.Claims.FirstOrDefault(c => c.Type == "scope")?.Value.Contains(scope) == true));
                }
            });

        return services;
    }

    public static IServiceCollection AddClientSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var oauthOptions = OauthOptions.GetOauthOptions(configuration);
        if (oauthOptions?.Client == null)
        {
            return services;
        }

        var builder = services.AddDistributedMemoryCache()
            .AddClientCredentialsTokenManagement()
            .AddClient(SmusdiOptions.ServiceName, client =>
            {
                client.TokenEndpoint = oauthOptions.TokenEndpoint ?? $"{oauthOptions.Authority}/protocol/openid-connect/token";
                oauthOptions.Client.UpdateClientCredentialsClient(client);
            });

        if (oauthOptions.NamedClients != null)
        {
            foreach (var item in oauthOptions.NamedClients)
            {
                builder = builder.AddClient(item.Name, client =>
                {
                    client.TokenEndpoint = item.TokenEndpoint ?? $"{item.Authority}/protocol/openid-connect/token";
                    item.Client.UpdateClientCredentialsClient(client);
                });
            }
        }

        return services;
    }
}
