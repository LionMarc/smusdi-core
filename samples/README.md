# Samples

## Keycloak

To tests authentication/authorization, a Keycloak instance is required with the following settings:

- realm **SMUSDI**,
- client **smusdi-sample-swagger**:

    - type *Standard Flow*;
    - associated optional scopes *scope1* and *scope2*;
    - redicrect url: http://localhost:5100/swagger/oauth2-redirect.html;
    - web origins: *

## Postgresql

To add a migration:

```
dotnet dotnet-ef migrations add <migration-name> -p PostgreSqlMigration\PostgreSqlMigration.csproj -s Smusdi.Sample\Smusdi.Sample.csproj
```