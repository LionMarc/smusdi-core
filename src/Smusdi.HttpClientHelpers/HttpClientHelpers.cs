using Duende.AccessTokenManagement;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Smusdi.HttpClientHelpers;

public static class HttpClientHelpers
{
    /// <summary>
    /// Registers a typed HTTP client with client-credentials authentication, a configurable handler lifetime,
    /// and a configurable Polly policy handler.
    /// </summary>
    /// <typeparam name="TClient">The typed HTTP client service.</typeparam>
    /// <typeparam name="TImplementation">The implementation of the typed HTTP client.</typeparam>
    /// <param name="services">The service collection to add the HTTP client to.</param>
    /// <param name="configureClient">The action used to configure the HTTP client.</param>
    /// <param name="clientName">The client name used to select client-credentials settings. Defaults to the configured main client name.</param>
    /// <param name="handlerLifetime">The lifetime of the HTTP message handler. Defaults to five minutes.</param>
    /// <param name="policyHandler">The Polly policy handler. Defaults to the basic transient-error retry policy.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddHttpClientWithClientCredentials<TClient, TImplementation>(
        this IServiceCollection services,
        Action<HttpClient> configureClient,
        string? clientName = null,
        TimeSpan? handlerLifetime = null,
        IAsyncPolicy<HttpResponseMessage>? policyHandler = null)
        where TClient : class
        where TImplementation : class, TClient
    {
        services
            .AddHttpClient<TClient, TImplementation>(configureClient)
            .AddClientCredentialsTokenHandler(ClientCredentialsClientName.Parse(clientName ?? HttpClientsOptions.DefaultClientName))
            .SetHandlerLifetime(handlerLifetime ?? TimeSpan.FromMinutes(5))
            .AddPolicyHandler(policyHandler ?? HttpClientPolicies.GetBasicPolicy());

        return services;
    }
}
