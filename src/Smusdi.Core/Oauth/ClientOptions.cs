namespace Smusdi.Core.Oauth;

public sealed class ClientOptions
{
    public required string ClientId { get; set; }

    public required string ClientSecret { get; set; }

    public string Scopes { get; set; } = string.Empty;
}
