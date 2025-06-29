@integration
Feature: FileSystem

This feature is used to tests the reqnroll steps associated to the file system mock.
I decide to use [System.IO.Abstractions](https://github.com/TestableIO/System.IO.Abstractions) in services and System.IO.Abastractions.TestingHelpers in tests.

@arg:use-file-system-mock=true
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

@arg:use-file-system-mock=true
Scenario: Removing a file and testing that it exists
    Given the file "K:/subfolder/test.txt" with content
        """
        Content of the file
        """
    When I delete the file "K:/subfolder/test.txt"
    Then the file "K:/subfolder/test.txt" does not exist

@arg:use-file-system-mock=true
Scenario: Testing that a folder is empty
    Given the file "K:/subfolder/test.txt" with content
        """
        Content of the file
        """
    When I delete the file "K:/subfolder/test.txt"
    Then the folder "K:/subfolder" is empty

@arg:use-file-system-mock=true
Scenario: Testing that a folder is not empty
    When I create the file "K:/subfolder/test.txt" with content
        """
        Content of the file
        """
    Then the folder "K:/subfolder" is not empty

@with-random-file-system-root-directory
Scenario: Real file system and random root folder
    Given the file "subfolder/test.txt" with content
        """
        Content of the file
        """
    Then the mock file system is not used
    And the root directory is set
    And the file "subfolder/test.txt" exists
    And the file "subfolder/test.txt" has content
        """
        Content of the file
        """
