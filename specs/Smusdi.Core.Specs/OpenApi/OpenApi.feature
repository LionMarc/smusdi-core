@integration
Feature: Native OpenAPI

As an application using Smusdi.Core without Smusdi.Swagger
I want the native Microsoft OpenAPI document to be available

Scenario: Serve OpenAPI without enabling Swagger UI
    When I execute the GET request "/openapi/v1.json"
    Then I receive a "OK" status
    And the response is an OpenAPI document
    When I execute the GET request "/swagger/index.html"
    Then I receive a "NotFound" status
