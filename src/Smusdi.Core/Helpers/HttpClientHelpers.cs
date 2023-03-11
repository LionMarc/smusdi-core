namespace Smusdi.Core.Helpers;

public static class HttpClientHelpers
{
    public static IServiceCollection AddSecuredHttpClient<TClient, TImplementation>(this IServiceCollection services, Action<HttpClient> configureClient)
        where TClient : class
        where TImplementation : class, TClient
    {
        services
            .AddHttpClient<TClient, TImplementation>(configureClient)
            .AddClientCredentialsTokenHandler(SmusdiOptions.ServiceName)
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(HttpClientPolicies.GetBasicPolicy());

        return services;
    }
}
