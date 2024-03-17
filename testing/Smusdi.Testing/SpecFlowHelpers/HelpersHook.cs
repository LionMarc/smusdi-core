using Reqnroll;
using Reqnroll.Assist;

namespace Smusdi.Testing.SpecFlowHelpers;

[Binding]
public static class HelpersHook
{
    [BeforeTestRun]
    public static void RegisterRetrievers()
    {
        Service.Instance.ValueRetrievers.Register(new TimeOnlyRetriever());
    }
}
