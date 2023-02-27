using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Smusdi.Testing.SpecFlowHelpers;

[Binding]
public sealed class HelpersHook
{
    [BeforeTestRun]
    public static void RegisterRetrievers()
    {
        Service.Instance.ValueRetrievers.Register(new TimeOnlyRetriever());
    }
}
