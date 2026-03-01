using Reqnroll;

namespace Smusdi.Testing.Http;

[Binding]
/// <summary>
/// Reqnroll steps for configuring claims on the test identity used by the TestAuthHandler.
/// </summary>
public sealed class ClaimsSteps(SmusdiServiceTestingSteps steps)
{
    /// <summary>
    /// Given step: registers the claims described in the provided data table into the
    /// <see cref="ClaimsProvider"/> for the current test scenario.
    /// </summary>
    /// <param name="dataTable">A table describing claims to add to the test identity.</param>
    [Given(@"identity with claims")]
    public void GivenIdentityWithClaims(DataTable dataTable)
    {
        var claimsProvider = steps.GetService<ClaimsProvider>();
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
