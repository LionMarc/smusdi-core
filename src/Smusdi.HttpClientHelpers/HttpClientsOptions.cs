using Microsoft.Extensions.Configuration;

namespace Smusdi.HttpClientHelpers;

public sealed record HttpClientsOptions(HttpClientOptions MainClient, Dictionary<string, HttpClientOptions>? NamedClients = null)
{
    public const string ConfigurationSection = "HttpClientsOptions";
    public const string DefaultClientName = "default_oauth_client";

    public static HttpClientsOptions? GetHttpClientsOptions(IConfiguration configuration) => configuration.GetSection(ConfigurationSection).Get<HttpClientsOptions>();
}
