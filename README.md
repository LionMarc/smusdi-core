# smusdi-core

Base .NET libraries for all the services of the application.

## Tests and coverage

```
dotnet test --collect:"XPlat Code Coverage" -r results
dotnet tool run reportgenerator -reports:"results/**/*.xml" -targetdir:coveragereport -reporttypes:htm
```

> *reportgenerator* is installed as a local tool
