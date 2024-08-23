using System.Globalization;
using Microsoft.Extensions.Time.Testing;
using Reqnroll;

namespace Smusdi.Testing.Clock;

[Binding]
public sealed class ClockSteps(SmusdiServiceTestingSteps steps)
{
    private readonly SmusdiServiceTestingSteps steps = steps;

    [Given(@"the system clock ""(.*)""")]
    [When(@"the system clock is set to {string}")]
    public void GivenTheSystemClock(string p0)
    {
        var fakeTimeProvider = this.steps.SmusdiTestingService.GetService<TimeProvider>() as FakeTimeProvider;
        fakeTimeProvider?.SetUtcNow(DateTimeOffset.Parse(p0, CultureInfo.InvariantCulture));
    }
}
