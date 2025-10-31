@arg:use-file-system-mock=false @integration
Feature: Rewrite url

File *urlRewriteRules.txt* is defined with content:

```
RewriteRule ^/to_redirect(.*) /target_of_rewrite$1
```

Scenario: Return result of target of rewrite rule
    When I execute the GET request "/to_redirect"
    Then I receive a "OK" status
    And I receive the response
        """
        {
            "endPoint":  "/target_of_rewrite"
        }
        """
