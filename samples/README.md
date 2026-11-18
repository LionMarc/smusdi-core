# Samples

## Local dependencies

From the repository root, start PostgreSQL and Keycloak:

```
docker compose -f docker/docker-compose.yml up -d
```

PostgreSQL is available on `localhost:5432` with user `postgres` and password
`postgrespw`. The integration tests use these defaults and create a temporary
database per scenario.

Keycloak is available at `http://localhost:5200` and `http://localhost:8080`.
The imported realms and clients match the sample configuration. The admin
console is at `http://localhost:5200/admin` (`admin` / `admin`); the `SMUSDI`
realm also includes the sample user `sample` / `sample` for interactive sign-in.
The `docker` folder contains the realm imports and PostgreSQL initialization
script.

Stop the services with:

```
docker compose -f docker/docker-compose.yml down
```

Add `-v` to `down` to remove the PostgreSQL data volume and reset the local
database and imported realms.

## Keycloak

To test authentication/authorization, use the `SMUSDI` realm and client
`smusdi-sample-swagger` created by the Compose setup:

- type *Standard Flow*;
- associated optional scopes *scope1* and *scope2*;
- redirect URL: `http://localhost:5100/swagger/oauth2-redirect.html`;
- web origins: `*`.

## Postgresql

To add a migration:

```
dotnet dotnet-ef migrations add <migration-name> -p PostgreSqlMigration\PostgreSqlMigration.csproj -s Smusdi.Sample\Smusdi.Sample.csproj
```