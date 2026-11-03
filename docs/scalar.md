# Scalar API Reference

Scalar is an optional alternative to Swagger UI. Reference the `Smusdi.Scalar` project/package to enable the API reference at **http://<host:port>/scalar**. It displays the native Microsoft OpenAPI documents exposed by Smusdi.Core at `/openapi/{documentName}.json`.

The Scalar UI is configured automatically for the application's API versioning mode:

- With API versioning enabled, Scalar lists each discovered OpenAPI document.
- With `smusdi:noVersioning` set to `true`, Scalar displays the `v1` document.

For example, to use Scalar in an application:

```sh
dotnet add package Smusdi.Scalar
```

When [reverse proxy path-prefix configuration](./webapi-config.md#reverse-proxy-path-prefixes) is enabled, Scalar and its OpenAPI requests are available under the configured prefix.

## Scalar Agent

The Scalar Agent is disabled by default so the API reference does not send API data to Scalar's AI service. To enable it, explicitly set:

```json
{
  "scalar": {
    "enableAgent": true
  }
}
```

This setting only controls the Scalar Agent. The API reference still loads its OpenAPI document from the Smusdi service, and OAuth flows still communicate with the configured identity provider.

## OpenID Connect metadata

When using `Smusdi.Security`, the OpenAPI document includes the OAuth2 authorization-code flow and an OpenID Connect security scheme. Scalar reads the identity provider's discovery URL from the latter. For `oauth.mainAuthority` and `oauth.additionalAuthorities`, Smusdi derives the URL as `{authorityUrl}/.well-known/openid-configuration`; for a JWKS authority, it derives it when the URL ends with `/protocol/openid-connect/certs`.

If the discovery endpoint does not follow that convention, set `openIdConnectUrl` on that authority:

```json
{
  "oauth": {
    "mainAuthority": {
      "name": "identity-provider",
      "url": "https://identity.example.com/issuer",
      "openIdConnectUrl": "https://identity.example.com/custom/.well-known/openid-configuration"
    }
  }
}
```

The configured URL must be an absolute HTTP or HTTPS URL. The existing OAuth2 flow remains available for Swagger UI and other OpenAPI clients.
