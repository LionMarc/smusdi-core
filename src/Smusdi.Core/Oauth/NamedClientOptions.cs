namespace Smusdi.Core.Oauth;

public sealed class NamedClientOptions
{
    public required string Name { get; set; }

    public required string Authority { get; set; }

    public string? TokenEndpoint { get; set; }

    public required ClientOptions Client { get; set; }
}
