using Smusdi.Core.Helpers;

namespace Smusdi.Testing.Clock;

public class ClockMock : IClock
{
    public DateTimeOffset UtcNow { get; set; } = DateTimeOffset.UtcNow;
}
