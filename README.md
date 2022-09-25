# smusdi-core
![build workflow](https://github.com/LionMarc/smusdi-core/actions/workflows/build.yml/badge.svg)
[![license](https://img.shields.io/badge/License-MIT-purple.svg)](LICENSE)
[![HitCount](https://hits.dwyl.com/LionMarc/smusdi-core.svg?style=flat-square)](http://hits.dwyl.com/LionMarc/smusdi-core)

Base .NET libraries for all the services of the application.

## Tests and coverage

```
dotnet test --collect:"XPlat Code Coverage" -r results
dotnet tool run reportgenerator -reports:"results/**/*.xml" -targetdir:coveragereport -reporttypes:htm
```

> *reportgenerator* is installed as a local tool
