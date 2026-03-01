using System.Globalization;
using Microsoft.Extensions.Time.Testing;
using Reqnroll;

namespace Smusdi.Testing.Clock;

[Binding]
public sealed class ClockSteps(SmusdiServiceTestingSteps steps)
{
    /// <summary>
    /// Given/When step: sets the fake system clock to the specified UTC datetime string.
    /// </summary>
    /// <param name="now">The UTC datetime string to set the system clock to.</param>
    [Given(@"the system clock {string}")]
    [When(@"the system clock is set to {string}")]
    public void GivenTheSystemClock(string now)
    {
        var fakeTimeProvider = steps.GetService<TimeProvider>() as FakeTimeProvider;
        fakeTimeProvider?.SetUtcNow(DateTimeOffset.Parse(now, CultureInfo.InvariantCulture));
    }
}
