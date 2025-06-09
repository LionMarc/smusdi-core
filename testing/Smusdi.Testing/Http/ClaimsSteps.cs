using Reqnroll;

namespace Smusdi.Testing.Http;

[Binding]
public sealed class ClaimsSteps(SmusdiServiceTestingSteps steps)
{
    [Given(@"identity with claims")]
    public void GivenIdentityWithClaims(DataTable dataTable)
    {
        var claimsProvider = steps.SmusdiTestingService.GetService<ClaimsProvider>();
        if (claimsProvider == null)
        {
            return;
        }

        var claims = dataTable.CreateSet<ClaimDesc>();
        foreach (var claim in claims)
        {
            claimsProvider.AddClaim(claim);
        }
    }
}
