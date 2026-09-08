using AwesomeAssertions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Reqnroll;
using Smusdi.Testing;

namespace Smusdi.PostgreSQL.Testing;

/// <summary>
/// Reqnroll step definitions for asserting PostgreSQL database state.
/// </summary>
[Binding]
public sealed class PostgreSqlSteps(SmusdiServiceTestingSteps steps)
{
    private readonly IConfiguration configuration = steps.GetRequiredService<IConfiguration>();

    /// <summary>
    /// Then step: asserts that a table with the specified name exists in the target database.
    /// </summary>
    /// <param name="tableName">The name of the table to check.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Then("the table {string} exists")]
    public async Task ThenTheTableExists(string tableName)
    {
        var connectionString = this.configuration.GetValue<string>(Constants.ConnectionStringSettingsPath);
        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        using var command = connection.CreateCommand();
#pragma warning disable S2077 // SQL queries should not be dynamically formatted
        command.CommandText = $"SELECT EXISTS(SELECT FROM pg_tables WHERE tablename='{tableName}')";
#pragma warning restore S2077 // SQL queries should not be dynamically formatted
        var res = await command.ExecuteScalarAsync();
        res.Should().Be(true);
    }

    /// <summary>
    /// Then step: asserts that a table with the specified name does not exist in the target database.
    /// </summary>
    /// <param name="tableName">The name of the table that should not exist.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Then("the table {string} does not exist")]
    public async Task ThenTheTableDoesNotExist(string tableName)
    {
        var connectionString = this.configuration.GetValue<string>(Constants.ConnectionStringSettingsPath);
        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        using var command = connection.CreateCommand();
#pragma warning disable S2077 // SQL queries should not be dynamically formatted
        command.CommandText = $"SELECT EXISTS(SELECT FROM pg_tables WHERE tablename='{tableName}')";
#pragma warning restore S2077 // SQL queries should not be dynamically formatted
        var res = await command.ExecuteScalarAsync();
        res.Should().Be(false);
    }
}
