using Microsoft.IdentityModel.Tokens;

namespace Smusdi.Security;

public interface ISigningKeysLoader
{
    IEnumerable<SecurityKey> LoadSigningKeys(string jwksUrl);
}
