namespace Smusdi.Core.Oauth;

public sealed record OauthAuthority(string Name, string Url, string Audience = "account");
