Feature: AppsettingsFolderSelection

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

    Scenario: Should load from the current working directory when SMUSDI_APPSETTINGS_FOLDER is not set
        Given the service initialized
        When I start the service
        Then the read folder parameter is "current"
    
    Scenario: Should load from the sub-folder directory when SMUSDI_APPSETTINGS_FOLDER is set to sub-folder
        Given the environment variable "SMUSDI_APPSETTINGS_FOLDER" set to "sub-folder"
        And the service initialized
        When I start the service
        Then the read folder parameter is "sub-folder"
