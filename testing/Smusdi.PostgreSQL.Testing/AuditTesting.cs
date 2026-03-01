using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using Reqnroll;
using Smusdi.PostgreSQL.Audit;
using Smusdi.Testing;
using Smusdi.Testing.AwesomeAssertionsHelpers;
using Smusdi.Testing.Database;

namespace Smusdi.PostgreSQL.Testing;

/// <summary>
/// Reqnroll step definitions for audit-related scenarios targeting the PostgreSQL database.
/// </summary>
[Binding]
public sealed class AuditTesting(SmusdiServiceTestingSteps smusdiServiceTestingSteps)
{
    /// <summary>
    /// Then step: asserts that the audit records present in the database match the provided table.
    /// </summary>
    /// <param name="table">The expected records represented as a <see cref="DataTable"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Then(@"the audit records are registered")]
    public async Task ThenTheAuditRecordsAreRegistered(DataTable table)
    {
        await smusdiServiceTestingSteps.Execute<AuditDbContext>(async context =>
        {
            var records = await context.Set<AuditRecordDao>().ToListAsync();
            var expected = table.CreateSet<AuditRecordDao>().ToList();
            records.Should().BeEquivalentTo(expected, options => options.CheckOnlyTableHeaders(table).CompareDateTimeAsUtc());
        });
    }
}
