@integration
Feature: Native OpenAPI

As an application using Smusdi.Core without Smusdi.Swagger
I want the native Microsoft OpenAPI document to be available

@arg:scalar:title=SmusdiTestAPI @arg:scalar:description=OpenAPI_metadata_test @arg:scalar:contactEmail=api-email
Scenario: Serve OpenAPI without enabling Swagger UI
    When I execute the GET request "/openapi/v1.json"
    Then I receive a "OK" status
    And the response is an OpenAPI document
    And the OpenAPI info contains title "SmusdiTestAPI", description "OpenAPI_metadata_test", and contact email "api-email"
    When I execute the GET request "/swagger/index.html"
    Then I receive a "NotFound" status

@arg:smusdi:reverseProxy:server=https://api.example.com @arg:smusdi:reverseProxy:basePath=/my-service
Scenario: Serve OpenAPI behind a path-rewriting reverse proxy
    When I execute the GET request "/openapi/v1.json"
    Then I receive a "OK" status
    And the OpenAPI server is "https://api.example.com/my-service"
    When I execute the GET request "/my-service/openapi/v1.json"
    Then I receive a "OK" status
    And the OpenAPI server is "https://api.example.com/my-service"
    When I execute the GET request "/my-service/scalar/v1"
    Then I receive a "OK" status

@arg:scalar:documentTitle=Smusdi-Scalar-Docs
Scenario: Serve Scalar API Reference
    When I execute the GET request "/scalar/v1"
    Then I receive a "OK" status
    And the Scalar browser tab title is "Smusdi-Scalar-Docs"
    And the Scalar API Client button is hidden

@arg:scalar:showClientButton=true
Scenario: Show Scalar API Client button when configured
    When I execute the GET request "/scalar/v1"
    Then I receive a "OK" status
    And the Scalar API Client button is shown
