using Microsoft.Extensions.Configuration;

namespace Smusdi.Security;

public sealed class OauthOptions
{
    public const string ConfigurationSection = "oauth";

    /// <summary>
    /// Gets or sets the main oauth authority used to validate json web tokens in input requests.
    /// </summary>
    public required OauthAuthority MainAuthority { get; set; }

    /// <summary>
    /// Gets or sets the list of additional oauth authorities used to validate json web tokens in input requests.
    /// </summary>
    public IEnumerable<OauthAuthority> AdditionalAuthorities { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of scopes used to create authorization policies based on scope.
    /// </summary>
    public List<string>? Scopes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to hide the client secret input in the Swagger UI.
    /// </summary>
    public bool HideClientSecretInputInSwaggerUi { get; set; } = true;

    public static OauthOptions GetOauthOptions(IConfiguration configuration) => configuration.GetSection(ConfigurationSection).Get<OauthOptions>() ??
            throw new InvalidOperationException($"Configuration section '{ConfigurationSection}' is missing or invalid.");
}
