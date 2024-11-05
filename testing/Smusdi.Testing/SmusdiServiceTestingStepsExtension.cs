namespace Smusdi.Testing;

public static class SmusdiServiceTestingStepsExtension
{
    public static T? GetService<T>(this SmusdiServiceTestingSteps steps) => steps.SmusdiTestingService.GetService<T>();

    public static T GetRequiredService<T>(this SmusdiServiceTestingSteps steps)
        where T : notnull => steps.SmusdiTestingService.GetRequiredService<T>();

    public static IEnumerable<T> GetServices<T>(this SmusdiServiceTestingSteps steps) => steps.SmusdiTestingService.GetServices<T>();
}
