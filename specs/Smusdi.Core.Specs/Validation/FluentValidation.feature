@integration
Feature: FluentValidation

Scenario: Should return a 400 with the property name in camelcase when validation fails for a minimal api
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

Scenario: Should return a 400 with the property name in camelcase when validation fails for a standard controller
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

Scenario: Should return a 400 with the nested property name in camelcase when validation fails for a minimal api
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

        
Scenario: Should return a 400 with the nested property name in camelcase when validation fails for a standard controller
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
