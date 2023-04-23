@postgresql
Feature: DateTimeConverters

Background: 
    Given the environment variable "MIGRATE_DATABASE" set to "true"
    And the service initialized and started

Scenario: Saving and loading as utc date time value
    When I save the jobs
        | UtcStartTimestamp    | UtcEndTimestamp      |
        | 2023-04-12 02:00:00  |                      |
        | 2023-04-12T04:00:00Z | 2023-04-12T06:00:00Z |
    Then the jobs are stored
    """
    [
        {
            "id":1,
            "utcStartTimestamp" : "2023-04-12T02:00:00Z",
            "utcEndTimestamp": null
        },
        {
            "id":2,
            "utcStartTimestamp" : "2023-04-12T04:00:00Z",
            "utcEndTimestamp": "2023-04-12T06:00:00Z"
        }
    ]
    """
