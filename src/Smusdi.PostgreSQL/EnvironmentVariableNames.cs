namespace Smusdi.PostgreSQL;

public static class EnvironmentVariableNames
{
    public const string Host = "POSTGRES_HOST";
    public const string Port = "POSTGRES_PORT";
    public const string User = "POSTGRES_USER";
    public const string Password = "POSTGRES_PASSWORD";
    public const string Db = "POSTGRES_DB";
    public const string MigrateDatabse = "MIGRATE_DATABASE";

    public const string ConnectionStringTemplate = "server=%POSTGRES_HOST%;Port=%POSTGRES_PORT%;Database=%POSTGRES_DB%;UserId=%POSTGRES_USER%;Password=%POSTGRES_PASSWORD%;Trust Server Certificate=true;";
}
