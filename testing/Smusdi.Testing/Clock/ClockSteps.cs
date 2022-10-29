using Smusdi.Core.Helpers;
using TechTalk.SpecFlow;

namespace Smusdi.Testing.Clock;

[Binding]
public sealed class ClockSteps
{
    private readonly ClockMock? clockMock;

    public ClockSteps(SmusdiTestingService smusdiTestingService) => this.clockMock = smusdiTestingService.GetService<IClock>() as ClockMock;

    [Given(@"the system clock ""(.*)""")]
    public void GivenTheSystemClock(string p0)
    {
        if (this.clockMock != null)
        {
            this.clockMock.UtcNow = DateTimeOffset.Parse(p0);
        }
    }
}
