## Sample service with publication in a single file

### Compile and run

```cmd
dotnet publish SingleFilePublication.Service\SingleFilePublication.Service.csproj -r win-x64 --no-self-contained -o publish
cd publish
SingleFilePublication.Service.exe
```