namespace Smusdi.Core.Oauth;

public static class ServiceCollectionExtension
{
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
