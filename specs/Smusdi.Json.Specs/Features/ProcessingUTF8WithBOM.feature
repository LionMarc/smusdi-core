Feature: Processing UTF8 with BOM

Scenario: Split array from a file encoded in UTF8 with BOM
    When I split json array from file "Data/json_with_bom.json"
    Then the array items are found
        | Value                                            |
        | {"description":"testing file encoding with BOM"} |
