using Microsoft.EntityFrameworkCore;
using PostgreSqlMigration;
using Reqnroll;
using Smusdi.Core.Json;
using Smusdi.PostgreSQL.Audit;
using Smusdi.Testing;
using Smusdi.Testing.Database;

namespace Smusdi.PostgreSQL.Specs;

[Binding]
public class Steps(SmusdiServiceTestingSteps smusdiServiceTestingSteps)
{
    private readonly IAuditService auditService = smusdiServiceTestingSteps.GetRequiredService<IAuditService>();
    private readonly IJsonSerializer jsonSerializer = smusdiServiceTestingSteps.GetRequiredService<IJsonSerializer>();

    [When(@"I register the audit record")]
    public async Task WhenIRegisterTheAuditRecord(string input)
    {
        var inputRecord = this.jsonSerializer.Deserialize<AuditRecord<Job, User>>(input) ?? throw new InvalidOperationException("Input string cannot be parsed.");
        await this.auditService.AddRecord(
            inputRecord.Type,
            inputRecord.ObjectType,
            inputRecord.ObjectId,
            inputRecord.Payload,
            inputRecord.User);
    }

    [When(@"I save the jobs")]
    public Task WhenISaveTheJobs(Table table) => smusdiServiceTestingSteps.StoreItemsInDatabase<MigrationDbContext, JobDao>(table);

    [Then(@"the jobs are stored")]
    public async Task ThenTheJobsAreStored(string expected)
    {
        await smusdiServiceTestingSteps.Execute(async (MigrationDbContext context) =>
        {
            var stored = await context.Set<JobDao>().ToListAsync();
            this.jsonSerializer.Serialize(stored).ShouldBeSameJsonAs(expected);
        });
    }
}
