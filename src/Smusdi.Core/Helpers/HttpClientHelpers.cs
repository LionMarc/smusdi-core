﻿namespace Smusdi.Core.Helpers;

public static class HttpClientHelpers
{
    public static IServiceCollection AddHttpClientWithClientCredentials<TClient, TImplementation>(
        this IServiceCollection services,
        Action<HttpClient> configureClient,
        string? clientName = null)
        where TClient : class
        where TImplementation : class, TClient
    {
        services
            .AddHttpClient<TClient, TImplementation>(configureClient)
            .AddClientCredentialsTokenHandler(clientName ?? SmusdiOptions.ServiceName)
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(HttpClientPolicies.GetBasicPolicy());

        return services;
    }
}
