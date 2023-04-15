using Smusdi.Core.Json;
using Smusdi.PosgreSQL.Audit;
using Smusdi.Testing;
using TechTalk.SpecFlow;

namespace Smusdi.PostgreSQL.Specs;

[Binding]
public class Steps
{
    private readonly IAuditService auditService;
    private readonly IJsonSerializer jsonSerializer;

    public Steps(SmusdiServiceTestingSteps smusdiServiceTestingSteps)
    {
        this.auditService = smusdiServiceTestingSteps.SmusdiTestingService.GetService<IAuditService>() ?? throw new InvalidOperationException($"{nameof(IAuditService)} is not registered.");
        this.jsonSerializer = smusdiServiceTestingSteps.SmusdiTestingService.GetService<IJsonSerializer>() ?? throw new InvalidOperationException($"{nameof(IJsonSerializer)} is not registered.");
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
}
