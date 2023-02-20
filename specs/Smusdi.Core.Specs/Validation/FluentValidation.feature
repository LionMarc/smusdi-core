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

    Scenario: Should not return a 404 when calling a controller with wrong parameters and automatic validation is disabled
        Given the configuration in current folder
        """
        {
            "smusdi": {
                "disableAutomaticFluentValidation" : true
            }
        }
        """ 
        And the service initialized and started
        When I execute the POST request "/v1/projects" with content
        """
        {
            "name":"",
            "target": {
                "environment":"dev"
            }
        }
        """
        Then I receive a "OK" status
