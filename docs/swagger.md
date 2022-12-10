# Swagger settings

The swagger ui can be found at **http://<host:port>/swagger/index.html**.

The swagger ui can be configured in the appsettings file:
```json
{
    "swagger": {
        "Title": "%SMUSDI_SERVICE_NAME%",
        "description": "%SMUSDI_SERVICE_NAME%",
        "contactName": "TO_BE_SET",
        "contactMail": "TO_BE_SET",
        "reverseProxyBasePath": "api/smusdi-sample"
    }
}
```

| Parameter | Description | Default Value |
| --------- | ----------- | ------------- |
| title     | Title of the application | Name of the service|
| description | Description of the application | Name of the service|
| contactName | Support contact name | TO_BE_SET|
| contactMail | Support contact email | TO_BE_SET |
| reverseProxyBasePath | Use to properly setup the server path in case the service is behind a reverse proxy ||