using FluentValidation;

namespace Smusdi.Core.Validation;

public static class FluentValidationSetup
{
    public static void SetupGlobal()
    {
        ValidatorOptions.Global.PropertyNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;
    }
}
