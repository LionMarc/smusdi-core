using Duende.AccessTokenManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Extensibility;

namespace Smusdi.HttpClientHelpers;

public sealed class ServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        var options = HttpClientsOptions.GetHttpClientsOptions(configuration);
        if (options == null)
        {
            return services;
        }

        var builder = services.AddDistributedMemoryCache()
            .AddClientCredentialsTokenManagement()
            .AddClient(HttpClientsOptions.DefaultClientName, client =>
            {
                client.TokenEndpoint = new Uri(options.MainClient.TokenEndpoint);
                options.MainClient.UpdateClientCredentialsClient(client);
            });

        foreach (var item in options.NamedClients ?? [])
        {
            builder = builder.AddClient(item.Key, client =>
            {
                client.TokenEndpoint = new Uri(item.Value.TokenEndpoint);
                item.Value.UpdateClientCredentialsClient(client);
            });
        }

        return services;
    }
}
