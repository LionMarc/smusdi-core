Feature: Running tasks

RunTasks extension used to run a maximum number of tasks generated from an input enumerable.

Scenario: Run at most 3 tasks in parallel
    Given a list of 5 inputs
    When I run the tasks with a maximum of 3 tasks in parallel
    Then the logs execution are
        | log                         |
        | Requesting input 0          |
        | Starting processing input 0 |
        | Requesting input 1          |
        | Starting processing input 1 |
        | Requesting input 2          |
        | Starting processing input 2 |
        | Processing input 0 done     |
        | Requesting input 3          |
        | Starting processing input 3 |
        | Processing input 1 done     |
        | Requesting input 4          |
        | Starting processing input 4 |
        | Processing input 2 done     |
        | Processing input 3 done     |
        | Processing input 4 done     |
        | Execution done              |

Scenario: Input list with a length less than max tasks in parallel
    Given a list of 2 inputs
    When I run the tasks with a maximum of 3 tasks in parallel
    Then the logs execution are
        | log                         |
        | Requesting input 0          |
        | Starting processing input 0 |
        | Requesting input 1          |
        | Starting processing input 1 |
        | Processing input 0 done     |
        | Processing input 1 done     |
        | Execution done              |
