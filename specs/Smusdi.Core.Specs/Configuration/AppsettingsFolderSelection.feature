Feature: Application settings folder selection

The application must load appsettings file either from local folder, or from folder defined by the environment variable
SMUSDI_APPSETTINGS_FOLDER.

Background:
    Given the configuration in current folder
        """
        {
            "folder": "current"
        }
        """
    And the configuration in folder 'sub-folder'
        """
        {
            "folder": "sub-folder"
        }
        """

Scenario: Starting the service with variable SMUSDI_APPSETTINGS_FOLDER not set
    Given the service initialized
    When I start the service
    Then the read folder parameter is "current"
    
Scenario: Starting the service with variable SMUSDI_APPSETTINGS_FOLDER set to sub-folder
    Given the environment variable "SMUSDI_APPSETTINGS_FOLDER" set to "sub-folder"
    And the service initialized
    When I start the service
    Then the read folder parameter is "sub-folder"
