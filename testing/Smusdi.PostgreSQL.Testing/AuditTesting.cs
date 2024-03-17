using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Reqnroll;
using Smusdi.PostgreSQL.Audit;
using Smusdi.Testing;
using Smusdi.Testing.Database;
using Smusdi.Testing.FluentAssertionsHelpers;

namespace Smusdi.PostgreSQL.Testing;

[Binding]
public sealed class AuditTesting
{
    private readonly SmusdiTestingService smusdiTestingService;

    public AuditTesting(SmusdiServiceTestingSteps smusdiServiceTestingSteps) => this.smusdiTestingService = smusdiServiceTestingSteps.SmusdiTestingService;

    [Then(@"the audit records are registered")]
    public async Task ThenTheAuditRecordsAreRegistered(Table table)
    {
        await this.smusdiTestingService.Execute<AuditDbContext>(async (AuditDbContext context) =>
        {
            var records = await context.Set<AuditRecordDao>().ToListAsync();
            var expected = table.CreateSet<AuditRecordDao>().ToList();
            records.Should().BeEquivalentTo(expected, options => options.CheckOnlyTableHeaders(table).CompareDateTimeAsUtc());
        });
    }
}
