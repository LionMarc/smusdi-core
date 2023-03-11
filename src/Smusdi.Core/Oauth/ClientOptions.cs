namespace Smusdi.Core.Oauth;

public sealed class ClientOptions
{
    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string Scopes { get; set; } = string.Empty;
}
