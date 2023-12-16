Feature: Env file

Background: 
    Given all environment variables starting with "TEST_" removed

Scenario: Setup environment variables from default location
    Given the default .env file
        """
        TEST_ENV=my_env
        """
    And the service initialized
    When I start the service
    Then the environment variable "TEST_ENV" is set to "my_env"

Scenario: Setup environment variables from custom location
    Given the environment variable "SMUSDI_ENV_FILE" set to "my_custom_env_file"
    And the env file "my_custom_env_file" with content
        """
        TEST_ENV2=my_env
        """
    And the service initialized
    When I start the service
    Then the environment variable "TEST_ENV2" is set to "my_env"
