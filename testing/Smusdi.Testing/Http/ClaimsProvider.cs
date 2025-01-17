using System.Security.Claims;

namespace Smusdi.Testing.Http;

public class ClaimsProvider
{
    private readonly List<Claim> claims =
    [
        new("scope", "openid email profile"),
    ];

    public IEnumerable<Claim> Claims
    {
        get
        {
            return this.claims.AsReadOnly();
        }
    }

    public void AddClaim(ClaimDesc claimDesc)
    {
        if (claimDesc.Replace)
        {
            var claimToRemove = this.claims.Find(c => c.Type == claimDesc.Type);
            if (claimToRemove != null)
            {
                this.claims.Remove(claimToRemove);
            }
        }

        if (!claimDesc.Append)
        {
            this.claims.Add(new Claim(claimDesc.Type, claimDesc.Value));
            return;
        }

        var claim = this.claims.Find(c => c.Type == claimDesc.Type);
        if (claim != null)
        {
            this.claims.Remove(claim);
            this.claims.Add(new Claim(claim.Type, $"{claim.Value} {claimDesc.Value}"));
        }
        else
        {
            this.claims.Add(new Claim(claimDesc.Type, claimDesc.Value));
        }
    }

    public void ClearClaims()
    {
        this.claims.Clear();
        this.claims.Add(new("scope", "openid email profile"));
    }
}
