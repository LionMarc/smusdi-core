using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Smusdi.Security;

public sealed class OauthOptions
{
    public const string ConfigurationSection = "oauth";

    public OauthAuthority? MainAuthority { get; set; }

    public IEnumerable<OauthAuthority> AdditionalAuthorities { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of scopes used to create authorization policies based on scope.
    /// </summary>
    public List<string>? Scopes { get; set; }

    public static OauthOptions? GetOauthOptions(IConfiguration configuration)
    {
        OauthOptions? oauthOptions = null;
        if (configuration.GetSection(ConfigurationSection).Exists())
        {
            oauthOptions = new OauthOptions();
            configuration.Bind(ConfigurationSection, oauthOptions);
        }

        if (string.IsNullOrWhiteSpace(oauthOptions?.MainAuthority?.Url))
        {
            return null;
        }

        return oauthOptions;
    }
}
