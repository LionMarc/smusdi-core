# smusdi-core
![build workflow](https://github.com/LionMarc/smusdi-core/actions/workflows/build.yml/badge.svg)
[![license](https://img.shields.io/badge/License-MIT-purple.svg)](LICENSE)
[![HitCount](https://hits.dwyl.com/LionMarc/smusdi-core.svg?style=flat-square)](http://hits.dwyl.com/LionMarc/smusdi-core)

This project provides utility libraries for .NET services.

## Smusdi.Core

Bootstrapper for .NET services.

Just add the package as reference and set the following code in *program.cs* file of your api project:

```cs
using Smusdi.Core;

SmusdiService.InitAndRun(args);
```

The bootstrapper uses 2 environment variables:

- **SMUSDI_SERVICE_NAME**: the name of the service which will be used for the appsettings files, the logs files...
- **SMUSDI_APPSETTINGS_FOLDER**: this variable setup the folder in which the service can find the appsettings files. If not set, the appsettings files are supposed to be in the working directory.

> The bootstrapper load the following appsettings files:
> - appsettings.json
> - appsettings.{ASPNETCORE_ENVIRONMENT}.json
> - appsettings.{SMUSDI_SERVICE_NAME}.json
> - appsettings.{SMUSDI_SERVICE_NAME}.{ASPNETCORE_ENVIRONMENT}.json

A default *appsettings.json* file can be found in the *samples* folder.

## Tests and coverage

```
dotnet test --collect:"XPlat Code Coverage" -r results
dotnet tool run reportgenerator -reports:"results/**/*.xml" -targetdir:coveragereport -reporttypes:htm
```

> *reportgenerator* is installed as a local tool
