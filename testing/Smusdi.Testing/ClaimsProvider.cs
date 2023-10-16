using System.Security.Claims;

namespace Smusdi.Testing;

public class ClaimsProvider
{
    private readonly List<Claim> claims = new List<Claim>
    {
        new Claim("scope", "openid email profile"),
    };

    public IEnumerable<Claim> Claims
    {
        get
        {
            return this.claims.AsReadOnly();
        }
    }

    public void AddClaim(string type, string value, bool append)
    {
        if (!append)
        {
            this.claims.Add(new Claim(type, value));
            return;
        }

        var claim = this.claims.Find(c => c.Type == type);
        if (claim != null)
        {
            this.claims.Remove(claim);
            this.claims.Add(new Claim(claim.Type, $"{claim.Value} {value}"));
        }
        else
        {
            this.claims.Add(new Claim(type, value));
        }
    }
}
