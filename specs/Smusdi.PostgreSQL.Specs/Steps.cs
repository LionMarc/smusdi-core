using Microsoft.EntityFrameworkCore;
using PostgreSqlMigration;
using Reqnroll;
using Smusdi.Core.Json;
using Smusdi.PostgreSQL.Audit;
using Smusdi.Testing;
using Smusdi.Testing.Database;

namespace Smusdi.PostgreSQL.Specs;

[Binding]
public class Steps
{
    private readonly IAuditService auditService;
    private readonly IJsonSerializer jsonSerializer;
    private readonly SmusdiTestingService testingService;

    public Steps(SmusdiServiceTestingSteps smusdiServiceTestingSteps)
    {
        this.auditService = smusdiServiceTestingSteps.SmusdiTestingService.GetService<IAuditService>() ?? throw new InvalidOperationException($"{nameof(IAuditService)} is not registered.");
        this.jsonSerializer = smusdiServiceTestingSteps.SmusdiTestingService.GetService<IJsonSerializer>() ?? throw new InvalidOperationException($"{nameof(IJsonSerializer)} is not registered.");
        this.testingService = smusdiServiceTestingSteps.SmusdiTestingService;
    }

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
    public Task WhenISaveTheJobs(Table table) => this.testingService.StoreItemsInDatabase<MigrationDbContext, JobDao>(table);

    [Then(@"the jobs are stored")]
    public async Task ThenTheJobsAreStored(string expected)
    {
        await this.testingService.Execute(async (MigrationDbContext context) =>
        {
            var stored = await context.Set<JobDao>().ToListAsync();
            this.jsonSerializer.Serialize(stored).ShouldBeSameJsonAs(expected);
        });
    }
}
