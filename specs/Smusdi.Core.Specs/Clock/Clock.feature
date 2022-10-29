Feature: Clock

    Scenario: Should return the clock set in the mock
        Given the service initialized and started
        And the system clock "2022-10-29T13:45:54Z"
        When I execute the GET request "/testing-clock"
        Then I receive the response
        """
        {
            "utcNow": "2022-10-29T13:45:54Z"
        }
        """
