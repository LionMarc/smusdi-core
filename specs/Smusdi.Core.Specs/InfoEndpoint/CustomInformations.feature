@arg:use-file-system-mock=true
Feature: CustomInformations

In order to provide more information specific to some users
As a developer
I want to be able to add custom informations in the info endpoint

    Scenario: Should dispatch provided build information
        Given the environment variable "SMUSDI_SERVICE_NAME" set to "testing-app"
        And the environment variable "ASPNETCORE_ENVIRONMENT" set to "Staging"
        And the environment variable "SMUSDI_SERVICE_VERSION" set to "0.2.3"
        And the environment variable "SMUSDI_CUSTOM_INFOS_FOLDER" set to "K:/custom_infos"
        Given the service initialized and started
        And the file "K:/custom_infos/build.json" with content
        """
        {
            "branch": "main",
            "tag": "5.6.3"
        }
        """
        When I request the info endpoint
        Then I receive a "OK" status
        And I receive the response
        """
        {
            "serviceName":"testing-app",
            "serviceVersion":"0.2.3",
            "environment":"Staging",
            "build": {
                "branch": "main",
                "tag": "5.6.3"
            }
        }
        """
