namespace Smusdi.Security;

public sealed record OauthAuthority(
    string Name,
    string Url,
    string Audience = "account",
    bool RequireHttpsMetadata = true);
