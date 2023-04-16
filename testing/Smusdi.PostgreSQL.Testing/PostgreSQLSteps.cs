using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Smusdi.PostgreSQL.Audit;
using Smusdi.Testing;
using Smusdi.Testing.Database;
using Smusdi.Testing.FluentAssertionsHelpers;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Smusdi.PostgreSQL.Testing;

[Binding]
public sealed class PostgreSQLSteps
{
    [Then(@"the table ""(.*)"" exists")]
    public async Task ThenTheTableExists(string tableName)
    {
        using var connection = new NpgsqlConnection(Environment.ExpandEnvironmentVariables(EnvironmentVariableNames.ConnectionStringTemplate));
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT EXISTS(SELECT FROM pg_tables WHERE tablename='{tableName}')";
        var res = await command.ExecuteScalarAsync();
        res.Should().Be(true);
    }

    [Then(@"the table ""(.*)"" does not exist")]
    public async Task ThenTheTableDoesNotExist(string tableName)
    {
        using var connection = new NpgsqlConnection(Environment.ExpandEnvironmentVariables(EnvironmentVariableNames.ConnectionStringTemplate));
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT EXISTS(SELECT FROM pg_tables WHERE tablename='{tableName}')";
        var res = await command.ExecuteScalarAsync();
        res.Should().Be(false);
    }
}

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
