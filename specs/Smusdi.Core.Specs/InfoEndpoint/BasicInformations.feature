Feature: BasicInformations

In order to be sure to communicate with the right service
As a user
I want to get the service name, version and environment through a REST api

    Scenario: Should get the service name, version and environment when requesting the info endpoint
        Given the environment variable "SMUSDI_SERVICE_NAME" set to "testing-app"
        And the environment variable "ASPNETCORE_ENVIRONMENT" set to "Staging"
        And the environment variable "SMUSDI_SERVICE_VERSION" set to "0.2.3"
        And the service initialized and started
        When I request the info endpoint
        Then I receive a "OK" status
        And I receive the response
        """
        {
            "serviceName":"testing-app",
            "serviceVersion":"0.2.3",
            "environment":"Staging"
        }
        """
