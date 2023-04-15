@postgresql
Feature: Database initialization

If automatic migration is enabled, at startup of the service the migration must be applied.

Scenario: Starting the service with MIGRATE_DATABASE set to true
    Given the environment variable "MIGRATE_DATABASE" set to "true"
    And the service initialized and started
    Then the table "jobs" exists

Scenario: Starting the service with MIGRATE_DATABASE set to false
    Given the environment variable "MIGRATE_DATABASE" set to "false"
    And the service initialized and started
    Then the table "jobs" does not exist
