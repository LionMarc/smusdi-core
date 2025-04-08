using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    TimeSpan? CacheLifespan = null,
    bool ValidateIssuer = true,
    bool ValidateAudience = true,
    string? AuthorizationHeader = null)
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
            x.TokenValidationParameters.ValidateIssuer = this.ValidateIssuer;
            x.TokenValidationParameters.ValidateAudience = this.ValidateAudience;

            if (this.Type == OauthAuthorityType.Jwks)
            {
                x.Configuration = new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration()
                {
                    JwksUri = this.Url,
                };
            }

            if (!string.IsNullOrWhiteSpace(this.AuthorizationHeader))
            {
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers[this.AuthorizationHeader].FirstOrDefault();
                        if (!string.IsNullOrEmpty(token))
                        {
                            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                token = token["Bearer ".Length..].Trim();
                            }

                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                };
            }
        });
    }
}
