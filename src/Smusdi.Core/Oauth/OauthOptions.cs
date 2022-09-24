namespace Smusdi.Core.Oauth;

public class OauthOptions
{
    public string? Authority { get; set; }

    public List<string>? Scopes { get; set; }

    public static OauthOptions? GetOauthOptions(IConfiguration configuration)
    {
        OauthOptions? oauthOptions = null;
        if (configuration.GetSection("oauth").Exists())
        {
            oauthOptions = new OauthOptions();
            configuration.Bind("oauth", oauthOptions);
        }

        if (oauthOptions == null || string.IsNullOrWhiteSpace(oauthOptions.Authority))
        {
            return null;
        }

        return oauthOptions;
    }
}
