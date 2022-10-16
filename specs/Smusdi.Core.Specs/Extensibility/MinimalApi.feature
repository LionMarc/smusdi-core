Feature: MinimalApi

As a developper I want to add my custom minimal API when I use the SmusdiService

    Scenario: Endpoint /extension setup in IWebApplicationConfigurator implementation should be accessible
        Given the service initialized and started
        When I execute the GET request "/extension"
        Then I receive a "OK" status
        And I receive the response
        """
        {
            "endPoint":"/extension",
            "purpose":"For testing"
        }
        """
