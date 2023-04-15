using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace Smusdi.PostgreSQL.Specs;

[Binding]
public class Steps
{
    private readonly ISpecFlowOutputHelper specFlowOutputHelper;

    public Steps(ISpecFlowOutputHelper specFlowOutputHelper) => this.specFlowOutputHelper = specFlowOutputHelper;

    [Then(@"the database is created")]
    public void ThenTheDatabaseIsCreated()
    {
        this.specFlowOutputHelper.WriteLine($"database {Environment.GetEnvironmentVariable(EnvironmentVariableNames.Db)}");
    }
}
