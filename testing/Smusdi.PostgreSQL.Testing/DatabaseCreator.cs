using Npgsql;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace Smusdi.PostgreSQL.Testing;

[Binding]
public sealed class DatabaseCreator
{
    public const int DatabaseCreatorHookOrder = HookAttribute.DefaultOrder - 1000;
    public const string TargetTag = "postgresql";

    private readonly string databaseName = $"smusdi_{Guid.NewGuid()}";
    private readonly ISpecFlowOutputHelper specFlowOutputHelper;

    private string? connectionString;

    public DatabaseCreator(ISpecFlowOutputHelper specFlowOutputHelper) => this.specFlowOutputHelper = specFlowOutputHelper;

    [BeforeScenario(TargetTag, Order = DatabaseCreatorHookOrder)]
    public async Task DatabaseCreation()
    {
        var host = Environment.GetEnvironmentVariable(EnvironmentVariableNames.Host) ?? "localhost";
        var port = Environment.GetEnvironmentVariable(EnvironmentVariableNames.Port) ?? "5432";
        var user = Environment.GetEnvironmentVariable(EnvironmentVariableNames.User) ?? "postgres";
        var password = Environment.GetEnvironmentVariable(EnvironmentVariableNames.Password) ?? "postgrespw";
        var db = "postgres";

        this.connectionString = $"server={host};Port={port};Database={db};UserId={user};Password={password};Trust Server Certificate=true;";
        NpgsqlConnection.ClearAllPools();
        using var connection = new NpgsqlConnection(this.connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        this.specFlowOutputHelper.WriteLine($"[DatabaseCreator] Creating database {this.databaseName}...");
        command.CommandText = $"CREATE DATABASE \"{this.databaseName}\"";
        await command.ExecuteNonQueryAsync();
        connection.Close();
        this.specFlowOutputHelper.WriteLine($"[DatabaseCreator] Database {this.databaseName} created.");

        Environment.SetEnvironmentVariable(EnvironmentVariableNames.Db, this.databaseName);
        Environment.SetEnvironmentVariable(EnvironmentVariableNames.Host, host);
        Environment.SetEnvironmentVariable(EnvironmentVariableNames.Port, port);
        Environment.SetEnvironmentVariable(EnvironmentVariableNames.User, user);
        Environment.SetEnvironmentVariable(EnvironmentVariableNames.Password, password);
    }

    [AfterScenario(TargetTag)]
    public async Task DeleteDatabase()
    {
        this.specFlowOutputHelper.WriteLine($"[DatabaseCreator] Deleting database {this.databaseName}...");
        NpgsqlConnection.ClearAllPools();
        using var connection = new NpgsqlConnection(this.connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"DROP DATABASE \"{this.databaseName}\"";
        await command.ExecuteNonQueryAsync();
        connection.Close();
        this.specFlowOutputHelper.WriteLine($"[DatabaseCreator] Database {this.databaseName} deleted.");
    }
}
