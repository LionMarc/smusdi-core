using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Smusdi.Testing;
using TechTalk.SpecFlow;

namespace Smusdi.PostgreSQL.Testing;

[Binding]
public sealed class PostgreSqlSteps(SmusdiServiceTestingSteps steps)
{
    private readonly IConfiguration configuration = steps.SmusdiTestingService.GetRequiredService<IConfiguration>();

    [Then(@"the table ""(.*)"" exists")]
    public async Task ThenTheTableExists(string tableName)
    {
        var connectionString = this.configuration.GetValue<string>(Constants.ConnectionStringSettingsPath);
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT EXISTS(SELECT FROM pg_tables WHERE tablename='{tableName}')";
        var res = await command.ExecuteScalarAsync();
        res.Should().Be(true);
    }

    [Then(@"the table ""(.*)"" does not exist")]
    public async Task ThenTheTableDoesNotExist(string tableName)
    {
        var connectionString = this.configuration.GetValue<string>(Constants.ConnectionStringSettingsPath);
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT EXISTS(SELECT FROM pg_tables WHERE tablename='{tableName}')";
        var res = await command.ExecuteScalarAsync();
        res.Should().Be(false);
    }
}
