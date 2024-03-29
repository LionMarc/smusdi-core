﻿Feature: Polymorphic deserialization

Scenario: Deserialization of polymorphic object with children using same discriminator
    Given the service initialized and started
    When I deserialize the workflow
    """
    {
        "stages": [
            {
                "type": "Build",
                "name": "first",
                "stages": [
                    {
                        "order" : 1,
                        "type": "Test",
                        "name": "Third"
                    }
                ]
            },
            {
                "order" : 2,
                "type": "Test",
                "name": "second"
            }
        ],
        "type" : "Standard"
    }
    """
    Then I get a valid workflow

Scenario: Deserialization of polymorphic array with children using same discriminator
    Given the service initialized and started
    When I deserialize the workflow list
    """
    [
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
        },
        {
            "type" : "Simplified",
            "stages": [
                {
                    "type": "Build"
                },
                {
                    "type": "Test"
                }
            ]
        }
    ]
    
    """
    Then I get a valid list of workflow
