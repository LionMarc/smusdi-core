# Swagger settings

Swagger UI is optional. Reference the `Smusdi.Swagger` project/package to enable the UI. The UI displays the native Microsoft OpenAPI documents generated and exposed by Smusdi.Core at `/openapi/{documentName}.json`; Swagger does not generate or serve a separate document.

The swagger ui can be found at **http://<host:port>/swagger/index.html**.

The swagger ui can be configured in the appsettings file:
```json
{
    "swagger": {
        "documentTitle": "%SMUSDI_SERVICE_NAME%"
    }
}
```

| Parameter | Description | Default Value |
| --------- | ----------- | ------------- |
| documentTitle | Browser tab title for the Swagger UI | Swagger UI |