@arg:specificSection:rootFolder=my-root-folder
Feature: Using tags to setup command line arguments

Scenario: Tag set on feature
    Given the service initialized
    When I start the service
    Then the configuration property "rootFolder" of the section "specificSection" is equal to "my-root-folder"

@arg:specificSection:rootFolder=my-local-folder @arg:custom:data=123=kt
Scenario: Tag set on scenario
    Given the service initialized
    When I start the service
    Then the configuration property "rootFolder" of the section "specificSection" is equal to "my-local-folder"
    And the configuration property "data" of the section "custom" is equal to "123=kt"
