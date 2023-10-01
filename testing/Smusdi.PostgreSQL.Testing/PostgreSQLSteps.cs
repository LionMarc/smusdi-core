using FluentAssertions;
using Npgsql;
using TechTalk.SpecFlow;

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
