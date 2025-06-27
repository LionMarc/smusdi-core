# Configuration parameters / appsettings

Configuration parameters from appsettings could be overridden for scenarios without any modification in the appsettings file.

## Override with the reqnroll step

```gherkin
{% include "../../specs/Smusdi.Core.Specs/Configuration/OverridingServiceSettingsFromCommandLine.feature" %}
```

## Override with tags

```gherkin
{% include "../../specs/Smusdi.Core.Specs/Configuration/UsingTagsToSetupCommandLineArguments.feature" %}
```