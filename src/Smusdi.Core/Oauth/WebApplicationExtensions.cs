namespace Smusdi.Core.Oauth;

public static class WebApplicationExtensions
{
    public static WebApplication UseSecurity(this WebApplication webApplication, IConfiguration configuration)
    {
        var oauthOptions = OauthOptions.GetOauthOptions(configuration);
        if (oauthOptions == null)
        {
            return webApplication;
        }

        webApplication.UseAuthentication();
        webApplication.UseAuthorization();

        return webApplication;
    }
}
