# smusdi-core
![build workflow](https://github.com/LionMarc/smusdi-core/actions/workflows/build.yml/badge.svg)
[![license](https://img.shields.io/badge/License-MIT-purple.svg)](LICENSE)
[![HitCount](https://hits.dwyl.com/LionMarc/smusdi-core.svg?style=flat-square)](http://hits.dwyl.com/LionMarc/smusdi-core)
[![NuGet stable version](https://badgen.net/nuget/v/Smusdi.Core)](https://nuget.org/packages/Smusdi.Core)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Smusdi.Core.svg)](https://www.nuget.org/packages/Smusdi.Core/)

This project provides utility libraries for .NET services.

## Smusdi.Core

Bootstrapper for .NET services.

Just add the package as reference and set the following code in *program.cs* file of your api project:

```cs
using Smusdi.Core;

await SmusdiService.InitAndRunAsync(args);
```

The bootstrapper uses the following environment variables:

- **SMUSDI_SERVICE_NAME**: the name of the service which will be used for the appsettings files, the logs files...
- **SMUSDI_APPSETTINGS_FOLDER**: this variable setup the folder in which the service can find the appsettings files. If not set, the appsettings files are supposed to be in the working directory;
- **SMUSDI_ENV_FILE**: path of *.env* file used to setup a list of environment variables. If not set, the service try to load variables from file **.env**. If file does exist, the service does nothing.

> The bootstrapper load the following appsettings files:
> - appsettings.json
> - appsettings.{ASPNETCORE_ENVIRONMENT}.json
> - appsettings.{SMUSDI_SERVICE_NAME}.json
> - appsettings.{SMUSDI_SERVICE_NAME}.{ASPNETCORE_ENVIRONMENT}.json

- **SMUSDI_SERVICE_VERSION**: used by the *info* endpoint to send the service version;
- **SMUSDI_CUSTOM_INFOS_FOLDER** used by the *info* endpoint to send custom informations;
- **SMUSDI_EXPAND_ENV_TWICE**: if set to true, when reading appsettings variable, we try to expand twice the environment variables in case a variable is set by using another one.

A default *appsettings.json* file can be found in the *samples* folder.

## Smusdi.Extensibility => Extension Points

The project provides the following extension points:

- **IServicesRegistrator** used to register custom services
  ```C#
  public interface IServicesRegistrator : IBaseServicesRegistrator
  {
  }
  
  // Used by Smusdi.Testing to allow overriding some services when testing application
  public interface ITestingServicesRegistrator : IBaseServicesRegistrator
  {
  }
  
  public interface IBaseServicesRegistrator
  {
      IServiceCollection Add(IServiceCollection services, IConfiguration configuration);
  }
  ```
- **IWebApplicationConfigurator** to add some configuration actions to the web application
  ```C#
  public interface IWebApplicationConfigurator
  {
      WebApplication Configure(WebApplication webApplication);
  }
  ```
- **IBeforeRun** to execute some actions after the initialization of the host and before it is started
  ```C#
  public interface IBeforeRun
  {
      Task Execute();
  }
  ```
  The implementations of **IBeforeRun** are registered as *scoped* and executed in a scope. They are only executed when calling the static method *SmusdiService.InitAndRunAsync(args)*, not when calling the method *SmusdiService.InitAndRun(args)*.

- **ISecurityConfigurator** to setup api security
  ```C#
  public interface ISecurityConfigurator : IBaseServicesRegistrator
  {
      IApplicationBuilder Configure(IApplicationBuilder app, IConfiguration configuration);
  }
  ```

> The implementations of these interfaces are automatically executed. There are discovered with [scrutor](https://github.com/khellang/Scrutor).

## Tests and coverage

```
dotnet test --collect:"XPlat Code Coverage" -r results
dotnet tool run reportgenerator -reports:"results/**/*.xml" -targetdir:coveragereport -reporttypes:html
```

> *reportgenerator* is installed as a local tool

## Publishing as single-file

As reflection is limited when publishing application as a single-file, you have to setup in the appsettings file the list of assemblies in which the controlles or the extensions classes can be found.

See the sample *SingleFilePublication.Service*.

```json
{
    "smusdi": {
        "assemblyNames": [
          "SingleFilePublication",
          "SingleFilePublication.SomeFeature"
        ]
      }
}
```

> If an assembly can not be found, it is ignored.

## Documentation

- [mkdocs](https://www.mkdocs.org/);
- [mkdocs-material](https://squidfunk.github.io/mkdocs-material/);
- [mkdocs-include-markdown-plugin](https://github.com/mondeja/mkdocs-include-markdown-plugin);
- [mkdocs-glightbox](https://github.com/blueswen/mkdocs-glightbox).


## Response compression

| appsettings parameter | description | default value |
| --------------------- | ----------- | ------------- |
| compressionDisabled | Response compression is active only when false | false |
| compressionDisabledForHttps | If true, the compression is not active for HTTPS call | false |
| compressionLevel | Enum defining the applied compression level | Fastest |


## Fluent Validation

By default, automatic fluent validation is activated. To disable this feature, set in **appsettings** the *disableAutomaticFluentValidation* option to true.

```json
{
    "smusdi": {
        "disableAutomaticFluentValidation": true
    }
}
```

## Smusdi.PostgreSQL

Connection string must be defined as
``` json
{
    "ConnectionStrings": {
        "postgresql": "server=%POSTGRES_HOST%;Port=%POSTGRES_PORT%;Database=%POSTGRES_DB%;UserId=%POSTGRES_USER%;Password=%POSTGRES_PASSWORD%;Trust Server Certificate=true;"
    }
}

```

The *Smusdi.PostgreSQL.Testing* package creates a new database for each scenario. To do that, it uses the default values below. These values can be overriden in a *.runsettings* file, for example.

| Variable          | Default value in Smusdi.PostgreSQL.Testing |
| ----------------- | ------------------------------------------ |
| POSTGRES_HOST     | localhost                                  |
| POSTGRES_PORT     | 5432                                       |
| POSTGRES_USER     | postgres                                   |
| POSTGRES_PASSWORD | postgrespw                                 |
| POSTGRES_DB       | postgres                                   |

The name of the created database is a **GUID**. The reqnroll hook set the environement variable **POSTGRES_DB** to that value after the database is created.

> The *postgres* database is the database used to create the database used in tests. It must exist.

The extension **GetPostgreSqlSchema** of **IConfiguration** gets the database schema name from the first non null value:

- configuration key *smusdi:postgreSqlSchema*;
- environment varaiable *SMUSDI_SERVICE_NAME*;
- constant *public*.

## Smusdi.HttpClientHelpers => HttpClient and Oauth2

The token is managed by the library **Duende.AccessTokenManagement**.

The configuration is made according to the appsettings section **HttpClientsOptions**.

### Default client

```json
{
  "HttpClientsOptions": {
    "MainClient": {
      "TokenEndpoint": "default_token_endpoint", // For standard openid, it is  {authority_url}/protocol/openid-connect/token
      "ClientId" : "identifier_of_the_client_for_the_provider",
      "ClientSecret": "client_password",
      "Scopes": "default requested scopes",
      "ClientCredentialStyle" : "AuthorizationHeader | PostBody | undefined"
    }
  }
}
```

These settings are used by the extension method **HttpClientHelpers.AddHttpClientWithClientCredentials** with a null *clientName* value.

### With named clients

```json
{
  "HttpClientsOptions": {
    "MainClient": {
      "TokenEndpoint": "default_token_endpoint", // For standard openid, it is  {authority_url}/protocol/openid-connect/token
      "ClientId" : "identifier_of_the_client_for_the_provider",
      "ClientSecret": "client_password",
      "Scopes": "default requested scopes",
      "ClientCredentialStyle" : "AuthorizationHeader | PostBody | undefined"
    },
    "NamedClients": {
      "name of the client to be used when registering HttpClient": {
        "TokenEndpoint": "token_endpoint for that client",
        "ClientId" : "identifier_of_the_client_for_the_provider",
        "ClientSecret": "client_password",
        "Scopes": "default requested scopes",
        "ClientCredentialStyle" : "AuthorizationHeader | PostBody | undefined"
      }
    }
  }
}
```

These settings are used by the extension method **HttpClientHelpers.AddHttpClientWithClientCredentials** with a *clientName* equal to the name of the configured client.

> The *MainClient* property must be always set. If not, the process throws at startup.

## Smusdi.Security => Oauth providers / JWT bearer

The service can validate input token against more than one provider.

Additional providers can be declared in the appsettings file in the *oauth* section.

```json
  "Oauth": {
    "Scopes": [
      "scope1",
      "scope2"
    ],
    "MainAuthority": {
      "Name": "Smusdi",
      "Url": "%SMUSDI_OAUTH_URL%",
      "RequireHttpsMetadata": false,
      "Audience": "account"
    },
    "AdditionalAuthorities": [
      {
        "Name": "PSG",
        "Url": "%PSG_OAUTH_URL%/protocol/openid-connect/certs",
        "Issuer": "%PSG_OAUTH_URL%",
        "RequireHttpsMetadata": false,
        "Audience": "account",
        "Type": "Jwks",
        "CacheLifespan": "01:00:00",
        "ValidateIssuer": true, // By default true. If false, issuer in token is not validated against expected one
        "ValidateAudience": true // By default true. If false, audience in token is not validated against expected one"
      }
    ]
  }
```
> The default Audience value is **account** for both default main authority and additional authority.

> The default RequireHttpsMetadata is true.

The *Name* of the authority is used as authentication scheme. When receiving a token, the scheme is selected based on the issuer set in the token. THe authority to be used is the one whiwh has as *Url* the issuer set in token.

The *Scopes* are used to create default authorization policies bases on token scopes.

There are 3 types of authorities:

- *Oauth*: all the information are loaded from *{Url}/.well-knwon/openid-configuration*. This is the default if *Type* is not set;
- *Jwks*: the signing keys are retrieved from *Url* and stored in cache for *CacheLifespan* lifespan;
- *Custom*: you must register yourself the strategy to be used.


See https://learn.microsoft.com/en-us/aspnet/core/security/authorization/limitingidentitybyscheme?view=aspnetcore-8.0#use-multiple-authentication-schemes for details.
