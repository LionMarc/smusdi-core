using Duende.AccessTokenManagement;
using Duende.IdentityModel.Client;

namespace Smusdi.Core.Oauth;

public sealed class ClientOptions
{
    public required string ClientId { get; set; }

    public required string ClientSecret { get; set; }

    public string Scopes { get; set; } = string.Empty;

    public ClientCredentialStyle? ClientCredentialStyle { get; set; } = null;

    public void UpdateClientCredentialsClient(ClientCredentialsClient client)
    {
        client.ClientId = this.ClientId;
        client.ClientSecret = this.ClientSecret;
        client.Scope = this.Scopes;

        if (this.ClientCredentialStyle != null)
        {
            client.ClientCredentialStyle = this.ClientCredentialStyle.Value;
        }
    }
}
