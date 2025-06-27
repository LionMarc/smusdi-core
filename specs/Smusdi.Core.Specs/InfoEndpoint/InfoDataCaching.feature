@arg:use-file-system-mock=true
Feature: InfoDataCaching

By default, data returned by the info endpoint is cached. But sometimes, when json stored in the info folder
can be updated at runtime, cache must be disabled.

Background: 
  Given the environment variable "SMUSDI_SERVICE_NAME" set to "testing-app"
  And the environment variable "ASPNETCORE_ENVIRONMENT" set to "Staging"
  And the environment variable "SMUSDI_SERVICE_VERSION" set to "0.2.3"
  And the environment variable "SMUSDI_CUSTOM_INFOS_FOLDER" set to "K:/custom_infos"

Scenario: Calling info with cache disabled
  Given the service initialized and started
  And the info cache disabled
  And the file "K:/custom_infos/build.json" with content
  """
  {
      "branch": "main",
      "tag": "5.6.3"
  }
  """
  And the info endpoint requested
  And the file "K:/custom_infos/build.json" with content
  """
  {
      "branch": "main",
      "tag": "5.6.4"
  }
  """
  When I request the info endpoint
  Then I receive a "OK" status
  And I receive the response
  """
  {
      "serviceName":"testing-app",
      "serviceVersion":"0.2.3",
      "environment":"Staging",
      "build": {
          "branch": "main",
          "tag": "5.6.4"
      }
  }
  """

Scenario: Calling info with cache enabled
  Given the service initialized and started
  And the info cache enabled
  And the file "K:/custom_infos/build.json" with content
  """
  {
      "branch": "main",
      "tag": "5.6.3"
  }
  """
  And the info endpoint requested
  And the file "K:/custom_infos/build.json" with content
  """
  {
      "branch": "main",
      "tag": "5.6.4"
  }
  """
  When I request the info endpoint
  Then I receive a "OK" status
  And I receive the response
  """
  {
      "serviceName":"testing-app",
      "serviceVersion":"0.2.3",
      "environment":"Staging",
      "build": {
          "branch": "main",
          "tag": "5.6.3"
      }
  }
  """
