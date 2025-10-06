using Duende.AccessTokenManagement;
using Duende.IdentityModel.Client;

namespace Smusdi.HttpClientHelpers;

public sealed record HttpClientOptions(
    string TokenEndpoint,
    string ClientId,
    string ClientSecret,
    string Scopes = "",
    ClientCredentialStyle? ClientCredentialStyle = null)
{
    public void UpdateClientCredentialsClient(ClientCredentialsClient client)
    {
        client.ClientId = Duende.AccessTokenManagement.ClientId.Parse(this.ClientId);
        client.ClientSecret = Duende.AccessTokenManagement.ClientSecret.Parse(this.ClientSecret);
        client.Scope = Scope.Parse(this.Scopes);

        if (this.ClientCredentialStyle != null)
        {
            client.ClientCredentialStyle = this.ClientCredentialStyle.Value;
        }
    }
}
