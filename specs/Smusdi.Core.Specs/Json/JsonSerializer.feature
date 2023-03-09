Feature: JsonSerializer

Scenario: Deserialization from a stream
    Given the service initialized and started
    And the file "K:/content.json" with content
    """
    {
        "stages": [
            {
                "type": "Build"
            },
            {
                "type": "Test"
            }
        ],
        "type" : "Standard"
    }
    """
    When I deserialize the workflow from the file "K:/content.json"
    Then I get the right workflow

Scenario: Serialization to a stream
    Given the service initialized and started
    When I serialize a standard workflow with a build stage to the file "K:/content.json"
    Then the file "K:/content.json" has content
    """
    {"type":"Standard","stages":[{"type":"Build"}]}
    """
