Feature: Polymorphic serialization

Background: 
    Given the service initialized and started

Scenario: Serialization of polymorphic object with children using same discriminator
    Given the deserialized workflow "test"
    """
    {
        "stages": [
            {
                "type": "Build",
                "name": "first",
                "stages": [
                    {
                        "type": "Test",
                        "name": "Third"
                    }
                ]
            },
            {
                "type": "Test",
                "name": "second",
                "order": 4
            },
            {
                "type": "List",
                "name" : "List",
                "stages": [
                    {
                        "type": "Build",
                        "name": "first",
                        "stages": [
                            {
                                "type": "Test",
                                "name": "Third",
                                "order": 3
                            },
                            {
                                "type": "Test",
                                "name": "second",
                                "order": 4
                            }
                        ]
                    }
                ]
            }
        ],
        "type" : "Standard"
    }
    """
    When I serialized the workflow "test"
    Then I get the result
    """
    {
        "stages": [
            {
                "type": "Build",
                "name": "first",
                "stages": [
                    {
                        "order": 0,
                        "type": "Test",
                        "name": "Third"
                    }
                ]
            },
            {
                "type": "Test",
                "name": "second",
                "order": 4
            },
            {
                "type": "List",
                "name" : "List",
                "stages": [
                    {
                        "type": "Build",
                        "name": "first",
                        "stages": [
                            {
                                "type": "Test",
                                "name": "Third",
                                "order": 3
                            },
                            {
                                "type": "Test",
                                "name": "second",
                                "order": 4
                            }
                        ]
                    }
                ]
            }
        ],
        "type" : "Standard"
    }
    """
