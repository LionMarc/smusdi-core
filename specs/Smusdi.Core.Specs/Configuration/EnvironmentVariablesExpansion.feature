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

Scenario Outline: Property containing an environement variable that references an environement variable
    Given the configuration in current folder
        """
        {
            "serviceUrl": "http://%TARGET_SERVER%:%TARGET_PORT%"
        }
        """
    And the environment variable "ENV" set to "int"
    And the environment variable "SMUSDI_EXPAND_ENV_TWICE" set to "<SMUSDI_EXPAND_ENV_TWICE>"
    And the environment variable "TARGET_SERVER" set to "myserver.%ENV%.fr.world"
    And the environment variable "TARGET_PORT" set to "23457"
    And the service initialized
    When I start the service
    Then the value of the config property "serviceUrl" is "<SERVICE_URL>"

    Examples: 
        | SMUSDI_EXPAND_ENV_TWICE | SERVICE_URL                          |
        |                         | http://myserver.%ENV%.fr.world:23457 |
        | dd                      | http://myserver.%ENV%.fr.world:23457 |
        | True                    | http://myserver.int.fr.world:23457   |
        | False                   | http://myserver.%ENV%.fr.world:23457 |
