Feature: Pipeline execution

Background: 
    Given the service initialized and started

Scenario: Execute all steps of the pipeline 
    Given the pipeline with the steps
        | Name  |
        | step1 |
        | step2 |
        | step3 |
    When I run the pipeline
    Then the steps have been executed
        | Name  |
        | step1 |
        | step2 |
        | step3 |
    And the pipeline is in state "Done"
    And the finally action has been called

Scenario: Execute the decorator for each step 
    Given the pipeline with the steps
        | Name  |
        | step1 |
        | step2 |
        | step3 |
    And the attached decorator
    When I run the pipeline
    Then the steps have been executed
        | Name  |
        | step1 |
        | step2 |
        | step3 |
    And the pipeline is in state "Done"
    And the finally action has been called
    And the decorator result is "step1-step2-step3"

Scenario: Running a pipeline with a step throwing an expection and no catch action defined 
    Given the pipeline with the steps
        | Name  | Throw |
        | step1 | false |
        | step2 | true  |
        | step3 | false |
    When I run the pipeline
    Then the steps have been executed
        | Name  |
        | step1 |
        | step2 |
    And the pipeline is in state "Fatal"
    And the finally action has been called

Scenario: Running a pipeline with a step throwing an expection and a catch action defined 
    Given the pipeline with the steps
        | Name  | Throw |
        | step1 | false |
        | step2 | true  |
        | step3 | false |
    And a catch action defined
    When I run the pipeline
    Then the steps have been executed
        | Name  |
        | step1 |
        | step2 |
    And the pipeline is in state "Ko"
    And the catch action has been called
    And the finally action has been called

Scenario: Running a pipeline with a step cancelling the pipeline by throwing and a catch action defined 
    Given the pipeline with the steps
        | Name  | Cancel |
        | step1 | false  |
        | step2 | true   |
        | step3 | false  |
    And a catch action defined
    When I run the pipeline
    Then the steps have been executed
        | Name  |
        | step1 |
        | step2 |
    And the pipeline is in state "Cancelled"
    And the catch action has not been called
    And the finally action has been called

Scenario: Running a pipeline with a step cancelling the pipeline by calling the CancelPipeline method 
    Given the pipeline with the steps
        | Name  | Cancel | CallCancelMethod |
        | step1 | false  | false            |
        | step2 | false  | true             |
        | step3 | false  | false            |
    When I run the pipeline
    Then the steps have been executed
        | Name  |
        | step1 |
        | step2 |
    And the pipeline is in state "Cancelled"
    And the finally action has been called

Scenario: Executing a pipeline with stages registered as keyed services
    Given the pipeline with the keyed steps
        | Name   |
        | keyed1 |
        | keyed2 |
        | keyed4 |
    When I run the pipeline
    Then the steps have been executed
        | Name  |
        | keyed1 |
        | keyed2 |
        | keyed4 |
