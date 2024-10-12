Feature: Splitting root json array

Scenario: Trying to split an empty input
    When I split the json array
        """
        """
    Then a "JsonArraySplitterException" exception is thrown
        """
        Nothing to read from input stream.
        """

Scenario: Trying to split a root object
    When I split the json array
        """
        {
            "label" : "test"
        }
        """
    Then a "JsonArraySplitterException" exception is thrown
        """
        Expected a root array, not an object.
        """

Scenario: Splitting an array of string
    When I split the json array
        """
        [
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"
        ]
        """
    Then a "JsonArraySplitterException" exception is thrown
        """
        Class must be used to split array of objects only.
        """

Scenario: Splitting array of simple objects
    When I split the json array
        """
        [
            {
                "type": "Build",
                "name": "first"
            },
            {
                "type": "Test",
                "name": "second"
            },
            {
                "type": "Publish",
                "name": "Third"
            }
        ]
        """
    Then the array items are found
        | Value                             |
        | {"type":"Build","name":"first"}   |
        | {"type":"Test","name":"second"}   |
        | {"type":"Publish","name":"Third"} |


Scenario: Splitting array with a complex object
    When I split the json array
        """
        [
            {
                "type": "Build",
                "name": "first"
            },
            {
                "label": "Second item",
                "price": 56.7878,
                "values": [
                    67.5,
                    78.9
                ],
                "histo": [
                    {
                        "date": "2024-10-10",
                        "price": 4.56
                    }
                ]
            },
            {
                "type": "Publish",
                "name": "Third"
            }
        ]
        """
    Then the array items are found
        | Value                                                                                                     |
        | {"type":"Build","name":"first"}                                                                           |
        | {"label":"Second item","price":56.7878,"values":[67.5,78.9],"histo":[{"date":"2024-10-10","price":4.56}]} |
        | {"type":"Publish","name":"Third"}                                                                         |