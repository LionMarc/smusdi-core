using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using Reqnroll;
using Smusdi.PostgreSQL.Audit;
using Smusdi.Testing;
using Smusdi.Testing.AwesomeAssertionsHelpers;
using Smusdi.Testing.Database;

namespace Smusdi.PostgreSQL.Testing;

[Binding]
public sealed class AuditTesting(SmusdiServiceTestingSteps smusdiServiceTestingSteps)
{
    [Then(@"the audit records are registered")]
    public async Task ThenTheAuditRecordsAreRegistered(Table table)
    {
        await smusdiServiceTestingSteps.Execute<AuditDbContext>(async (AuditDbContext context) =>
        {
            var records = await context.Set<AuditRecordDao>().ToListAsync();
            var expected = table.CreateSet<AuditRecordDao>().ToList();
            records.Should().BeEquivalentTo(expected, options => options.CheckOnlyTableHeaders(table).CompareDateTimeAsUtc());
        });
    }
}
