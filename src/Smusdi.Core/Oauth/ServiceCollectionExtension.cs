﻿using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Smusdi.Core.Oauth;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var oauthOptions = OauthOptions.GetOauthOptions(configuration);
        if (oauthOptions == null)
        {
            return services;
        }

        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
            {
                x.Audience = "account";
                x.Authority = oauthOptions.Authority;
                x.RequireHttpsMetadata = false;
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
                client.TokenEndpoint = $"{oauthOptions.Authority}/protocol/openid-connect/token";
                client.ClientId = oauthOptions.Client.ClientId;
                client.ClientSecret = oauthOptions.Client.ClientSecret;
                client.Scope = oauthOptions.Client.Scopes;
            });

        if (oauthOptions.NamedClients != null)
        {
            foreach (var item in oauthOptions.NamedClients)
            {
                builder = builder.AddClient(item.Name, client =>
                {
                    client.TokenEndpoint = $"{item.Authority}/protocol/openid-connect/token";
                    client.ClientId = item.Client.ClientId;
                    client.ClientSecret = item.Client.ClientSecret;
                    client.Scope = item.Client.Scopes;
                });
            }
        }

        return services;
    }
}
