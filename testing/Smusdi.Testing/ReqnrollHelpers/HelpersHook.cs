using Reqnroll;
using Reqnroll.Assist;

namespace Smusdi.Testing.ReqnrollHelpers;

[Binding]
/// <summary>
/// Reqnroll hook class used to register custom value retrievers before the test run.
/// </summary>
public static class HelpersHook
{
    /// <summary>
    /// Registers additional value retrievers (for example <see cref="TimeOnlyRetriever"/>)
    /// into Reqnroll's <c>Service.Instance.ValueRetrievers</c> before any tests run.
    /// </summary>
    [BeforeTestRun]
    public static void RegisterRetrievers() => Service.Instance.ValueRetrievers.Register(new TimeOnlyRetriever());
}
