Feature: Specific appsettings file

When a setting is overridden in the appsettings file specific to the current service, this value must be used instead of the
one defined in the shared file.

Scenario: Starting service with a setting overridden in the specific appsettings file
    Given the environment variable "SMUSDI_SERVICE_NAME" set to "myservice"
    And the configuration in current folder
        """
        {
            "folder": "current"
        }
        """
    And the configuration file "appsettings.myservice.json"
        """
        {
            "folder": "anotherone"
        }
        """
    And the service initialized
    When I start the service
    Then the read folder parameter is "anotherone"
