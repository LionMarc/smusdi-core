namespace Smusdi.Core.Oauth;

public sealed class OauthOptions
{
    public const string ConfigurationSection = "oauth";

    public string? Authority { get; set; }

    public string Audience { get; set; } = "account";

    public string? TokenEndpoint { get; set; }

    public IEnumerable<OauthAuthority> AdditionalAuthorities { get; set; } = [];

    public List<string>? Scopes { get; set; }

    public ClientOptions? Client { get; set; }

    public List<NamedClientOptions>? NamedClients { get; set; }

    public static OauthOptions? GetOauthOptions(IConfiguration configuration)
    {
        OauthOptions? oauthOptions = null;
        if (configuration.GetSection(ConfigurationSection).Exists())
        {
            oauthOptions = new OauthOptions();
            configuration.Bind(ConfigurationSection, oauthOptions);
        }

        if (oauthOptions == null || string.IsNullOrWhiteSpace(oauthOptions.Authority))
        {
            return null;
        }

        return oauthOptions;
    }
}
