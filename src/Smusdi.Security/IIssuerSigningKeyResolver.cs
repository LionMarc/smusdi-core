using Microsoft.IdentityModel.Tokens;

namespace Smusdi.Security;

public interface IIssuerSigningKeyResolver
{
    IEnumerable<SecurityKey> GetIssuerSigningKeys(string jwksUrl, string kid, TimeSpan cacheDuration);
}
