Feature: FluentValidation

    Scenario: Should return a 404 with the property name in camelcase when validation fails for a minimal api
        Given the service initialized and started
        When I execute the POST request "/projects" with content
        """
        {
            "name":"",
            "target": {
                "environment":"dev"
            }
        }
        """
        Then I receive a "BadRequest" status
        And I receive the validation errors
        """
        {
            "name": [
                "The name must be set."
            ]
        }
        """

    Scenario: Should return a 404 with the property name in camelcase when validation fails for a standard controller
        Given the service initialized and started
        When I execute the POST request "/v1/projects" with content
        """
        {
            "name":"",
            "target": {
                "environment":"dev"
            }
        }
        """
        Then I receive a "BadRequest" status
        And I receive the validation errors
        """
        {
            "name": [
                "The name must be set."
            ]
        }
        """

    Scenario: Should return a 404 with the nested property name in camelcase when validation fails for a minimal api
        Given the service initialized and started
        When I execute the POST request "/projects" with content
        """
        {
            "name":"test",
            "target": {
                "environment":""
            }
        }
        """
        Then I receive a "BadRequest" status
        And I receive the validation errors
        """
        {
            "target.environment": [
                "The environment must be set."
            ]
        }
        """

        
    Scenario: Should return a 404 with the nested property name in camelcase when validation fails for a standard controller
        Given the service initialized and started
        When I execute the POST request "/v1/projects" with content
        """
        {
            "name":"test",
            "target": {
                "environment":""
            }
        }
        """
        Then I receive a "BadRequest" status
        And I receive the validation errors
        """
        {
            "target.environment": [
                "The environment must be set."
            ]
        }
        """