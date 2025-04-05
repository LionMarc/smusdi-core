using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Smusdi.Security;

public enum OauthAuthorityType
{
    Oauth,
    Jwks,
    Custom,
}

public record OauthAuthority(
    string Name,
    string Url,
    string Audience = "account",
    bool RequireHttpsMetadata = true,
    string? Issuer = null,
    OauthAuthorityType Type = OauthAuthorityType.Oauth,
    TimeSpan? CacheLifespan = null)
{
    public static readonly TimeSpan DefaultCacheLifespan = TimeSpan.FromHours(12);

    public virtual AuthenticationBuilder AddJwtBearer(AuthenticationBuilder builder)
    {
        if (this.Type == OauthAuthorityType.Custom)
        {
            return builder;
        }

        return builder.AddJwtBearer(this.Name, x =>
        {
            x.Audience = this.Audience;
            x.Authority = this.Url;
            x.RequireHttpsMetadata = this.RequireHttpsMetadata;
            x.TokenValidationParameters.ValidIssuer = this.Issuer ?? this.Url;

            if (this.Type == OauthAuthorityType.Jwks)
            {
                x.Configuration = new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration()
                {
                    JwksUri = this.Url,
                };
            }
        });
    }
}
