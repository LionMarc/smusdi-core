Feature: Environment variables expansion in configuration settings

When a property uses an environement variable, the variable must be expanded before being sent to the application.

Scenario: Starting service with an property in appsettings.json using an environment variable
    Given the configuration in current folder
        """
        {
            "serviceUrl": "http://%TARGET_SERVER%:%TARGET_PORT%"
        }
        """
    And the environment variable "TARGET_SERVER" set to "myserver.fr.world"
    And the environment variable "TARGET_PORT" set to "23457"
    And the service initialized
    When I start the service
    Then the value of the config property "serviceUrl" is "http://myserver.fr.world:23457"
