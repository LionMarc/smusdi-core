using System.Globalization;
using Microsoft.Extensions.Time.Testing;
using TechTalk.SpecFlow;

namespace Smusdi.Testing.Clock;

[Binding]
public sealed class ClockSteps
{
    private readonly FakeTimeProvider? fakeTimeProvider;

    public ClockSteps(SmusdiServiceTestingSteps steps) => this.fakeTimeProvider = steps.SmusdiTestingService.GetService<TimeProvider>() as FakeTimeProvider;

    [Given(@"the system clock ""(.*)""")]
    public void GivenTheSystemClock(string p0)
    {
        if (this.fakeTimeProvider != null)
        {
            this.fakeTimeProvider.SetUtcNow(DateTimeOffset.Parse(p0, CultureInfo.InvariantCulture));
        }
    }
}
