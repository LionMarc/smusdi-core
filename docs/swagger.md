# Swagger settings

Swagger UI is optional. Reference the `Smusdi.Swagger` project/package to enable the UI. The UI displays the native Microsoft OpenAPI documents generated and exposed by Smusdi.Core at `/openapi/{documentName}.json`; Swagger does not generate or serve a separate document.

The swagger ui can be found at **http://<host:port>/swagger/index.html**.

When [reverse proxy path-prefix configuration](./webapi-config.md#reverse-proxy-path-prefixes) is enabled, the UI and its OpenAPI requests are available under that prefix.

The swagger ui can be configured in the appsettings file:
```json
{
    "swagger": {
        "documentTitle": "%SMUSDI_SERVICE_NAME%",
        "title": "%SMUSDI_SERVICE_NAME%",
        "description": "API documentation.",
        "contactEmail": "api@example.com"
    }
}
```

| Parameter | Description | Default Value |
| --------- | ----------- | ------------- |
| documentTitle | Browser tab title for the Swagger UI | Swagger UI |
| title | OpenAPI document title displayed in the UI | Generated from the service assembly and API version, if available |
| description | OpenAPI document description displayed in the UI | Generated from the service assembly, if available |
| contactEmail | Contact email included in the OpenAPI document | Not set |