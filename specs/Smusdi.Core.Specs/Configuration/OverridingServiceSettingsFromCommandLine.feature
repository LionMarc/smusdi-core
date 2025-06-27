Feature: Overriding service settings from command line

For some tests, it is very usefull to override a configuration parameter defined in specific appsettting file without having to use a dedicated file 
or environment variable.

With appsettings.json containing:

{
  "specificSection": {
    "rootFolder": "folders/root"
  }
}


Scenario: Override configuration parameters by passing value as command line argument
    Given the command line arguments
        | Field                      | Value          |
        | specificSection:rootFolder | my-root-folder |
    And the service initialized
    When I start the service
    Then the configuration property "rootFolder" of the section "specificSection" is equal to "my-root-folder"

Scenario: No override of the configuration parameter
    Given the service initialized
    When I start the service
    Then the configuration property "rootFolder" of the section "specificSection" is equal to "folders/root"
