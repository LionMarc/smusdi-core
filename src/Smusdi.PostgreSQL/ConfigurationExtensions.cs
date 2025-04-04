using Microsoft.Extensions.Configuration;
using Smusdi.Core;
using Smusdi.Extensibility;

namespace Smusdi.PostgreSQL;

public static class ConfigurationExtensions
{
    public const string PostgreSqlSchemaProperty = "smusdi:postgreSqlSchema";

    public static string GetPostgreSqlSchema(this IConfiguration configuration)
    {
        return configuration.GetValue<string>(PostgreSqlSchemaProperty) ?? Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiServiceNameEnvVar) ?? "public";
    }
}
