@postgresql
Feature: Audit

Background: 
    Given the environment variable "MIGRATE_DATABASE" set to "true"
    And the service initialized and started
    And the system clock "2023-04-15T14:12:00Z"

Scenario: The audit records table should be created
    Then the table "audit_records" exists

Scenario: Adding an audit record
    When I register the audit record
    """
    {
        "type": "Update",
        "objectId": "45",
        "objectType": "Job",
        "payLoad": {
            "id": 45,
            "title": "Test"
        },
        "user": {
            "id": "id001",
            "name": "Dupont"
        }
    }
    """
    Then the audit records are registered
        | Id | UtcCreationTimestamp | Type   | ObjectType | ObjectId | Payload                  | User                           |
        | 1  | 2023-04-15T14:12:00Z | Update | Job        | 45       | {"id":45,"title":"Test"} | {"id":"id001","name":"Dupont"} |
