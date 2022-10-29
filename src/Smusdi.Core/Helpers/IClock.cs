namespace Smusdi.Core.Helpers;

public interface IClock
{
    public DateTimeOffset UtcNow { get; }
}
