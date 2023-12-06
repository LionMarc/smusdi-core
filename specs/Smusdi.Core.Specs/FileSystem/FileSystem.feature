Feature: FileSystem

This feature is used to tests the specflow steps associated to the file system mock.
I decide to use [System.IO.Abstractions](https://github.com/TestableIO/System.IO.Abstractions) in services and System.IO.Abastractions.TestingHelpers in tests.

Background: 
    Given the service initialized and started

Scenario: Adding a file and testing that it exists
    When I create the file "K:/subfolder/test.txt" with content
    """
    Content of the file
    """
    Then the file "K:/subfolder/test.txt" exists
    And the file "K:/subfolder/test.txt" has content
    """
    Content of the file
    """

Scenario: Removing a file and testing that it exists
    Given the file "K:/subfolder/test.txt" with content
    """
    Content of the file
    """
    When I delete the file "K:/subfolder/test.txt"
    Then the file "K:/subfolder/test.txt" does not exist

Scenario: Testing that a folder is empty
    Given the file "K:/subfolder/test.txt" with content
    """
    Content of the file
    """
    When I delete the file "K:/subfolder/test.txt"
    Then the folder "K:/subfolder" is empty

Scenario: Testing that a folder is not empty
    When I create the file "K:/subfolder/test.txt" with content
    """
    Content of the file
    """
    Then the folder "K:/subfolder" is not empty
